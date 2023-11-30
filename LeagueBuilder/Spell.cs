using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using LeagueBuilder.Calc;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder;

public enum SpellSlot
{
    Q,
    E,
    W,
    R,
    Passive,
    Other
}

public enum SpellResolveMode
{
    Symbolic,
    Numerical
}

public class Spell
{
    public string Name;
    public string Title;
    public string Tooltip;

    public SpellSlot Slot;
    public ApiSpell Raw;
    public List<DamageType> DamageTypes;
    public List<string> SpellTags;
    public List<List<double>> EffectAmounts;
    public Dictionary<string, DataValue> DataValues;
    public List<IGameCalculation> SpellCalculations;
    public List<double> Cost;
    public double Coefficient;
    public double CastTime;
    public List<double> CooldownTime;
    public double DelayCastOffsetPercent;
    public double DelayTotalTimePercent;
    public List<double> CastRange;
    public List<int> MaxAmmo;
    public List<double> AmmoRechargeTime;
    public List<double> CastRadius;
    public List<string> CC;
    public List<string> PartialCC;
    public List<string> Replacements;

    private static Dictionary<string, DamageType> _dmgMap = new()
    {
        {"physicalDamage", DamageType.Physical},
        {"magicDamage", DamageType.Magic},
        {"trueDamage", DamageType.True}
    };
    private static Dictionary<string, EffectType> _effMap = new()
    {
        {"speed", EffectType.BonusMoveSpeedEffect},
        {"status", EffectType.CC},
        {"healing", EffectType.Heal},
        {"shield", EffectType.Shield},
        {"scaleArmor", EffectType.BonusArmor},
        {"scaleMR", EffectType.BonusMagicResist},
        {"scaleAD", EffectType.BonusAD},
        {"scaleAP", EffectType.BonusAP},
        {"lifeSteal", EffectType.BonusLifeSteal},
        {"scaleMana", EffectType.Mana}
    };

    private static List<string> _hardCc = ["Knock", "Charm", "Fear", "Taunt", "Sleep", "Stun", "Suppress"];
    private static List<string> _softCc = ["Blind", "Cripple", "Disarm", "Drowsy", "Ground", "Pull", "Nearsight", "Polymorph", "Root", "Silence", "Slow"];

    public Spell(ApiSpell spell, SpellSlot slot, StringResolver stringResolver)
    {
        Slot = slot;
        Raw = spell;

        Replacements = spell.MSpell.MClientData?.MTooltipData?.MLists?.LevelUp.Elements?
            .Select(el => el.Type.Replace("%d", el.TypeIndex.ToString())).ToList() ?? [];

        Name = spell.MScriptName;
        Title = stringResolver.Get(spell.MSpell.MClientData?.MTooltipData?.MLocKeys?.KeyName ?? "") ?? "";
        SpellTags = spell.MSpell.MSpellTags;
        Coefficient = spell.MSpell.MCoefficient;
        Cost = spell.MSpell.Mana ?? [];
        CastTime = spell.MSpell.MCastTime;
        CooldownTime = spell.MSpell.CooldownTime;
        DelayCastOffsetPercent = spell.MSpell.DelayCastOffsetPercent;
        MaxAmmo = spell.MSpell.MaxAmmo ?? [];
        AmmoRechargeTime = spell.MSpell.AmmoRechargeTime ?? [];

        DelayTotalTimePercent = spell.MSpell.DelayTotalTimePercent;
        CastRange = spell.MSpell.CastRange;
        CastRadius = spell.MSpell.CastRadius;
        string? tt = stringResolver.Get(spell.MSpell.MClientData?.MTooltipData?.MLocKeys?.KeyTooltip?.ToLower() ?? string.Empty);
        Tooltip = tt ?? "";//throw new Exception("no tooltip found");

        DamageTypes = new List<DamageType>();
        if (Tooltip.Contains("physicalDamage")) DamageTypes.Add(DamageType.Physical);
        if (Tooltip.Contains("magicDamage")) DamageTypes.Add(DamageType.Magic);
        if (Tooltip.Contains("trueDamage")) DamageTypes.Add(DamageType.True);
        CC = _hardCc.Where(c => Tooltip.Contains($"<status>{c}")).ToList();
        PartialCC = _softCc.Where(c => Tooltip.Contains($"<status>{c}")).ToList();

        EffectAmounts = spell.MSpell.MEffectAmount?.Select(m => m.Value).ToList() ?? [];
        DataValues = new Dictionary<string, DataValue>();
        foreach (RawDataValue value in spell.MSpell.MDataValues ?? [])
        {
            var dv = new DataValue()
            {
                Values = value.Values,
                BaseValue = value.MBaseP
            };
            if (value.MFormula is "" or "P")
            {
                DataValues[value.MName] = dv;
                continue;
            }

            dv.Formula = value.MFormula;
            DataValues[value.MName] = dv;
        }

        SpellCalculations = new List<IGameCalculation>();
        var ctx = new ParseContext(spell, DataValues);
        foreach (JsonProperty prop in spell.MSpell.MSpellCalculations?.RootElement.EnumerateObject() ?? [])
        {
            JsonElement spellCalc = prop.Value;
            CalculationType t = Utils.CalculationTypeFromString(spellCalc.GetProperty("__type").GetString() ?? string.Empty);
            IGameCalculation gc = t switch
            {
                CalculationType.GameCalculationType => new GameCalculation(),
                CalculationType.GameCalculationModifiedType => new GameCalculationModified(),
                CalculationType.GameCalculationConditional => new GameCalculationConditional(),
                _ => throw new ArgumentOutOfRangeException()
            };
            gc.FromJson(prop.Name, spellCalc, ctx);
            SpellCalculations.Add(gc);
        }
    }

    private SpellInstance GetInstance(int rank) => new SpellInstance(this, rank);

    public string ResolveSpellText(ChampionInstance champion, int rank, SpellResolveMode mode) =>
        ResolveSpellText(Tooltip, champion, rank, mode);

    private string? GetValue(string key, ChampionInstance champion, int rank, SpellResolveMode mode)
    {
        CalculationContext context = champion.GetCalculationContext(GetInstance(rank), champion.Champ.Resolver);
        foreach (IGameCalculation calculation in SpellCalculations)
        {
            if (!Utils.MatchesBinEntry(key, calculation.Name()))
                continue;
            return mode == SpellResolveMode.Symbolic ? calculation.String(context) : $"{Math.Round(calculation.Value(context))}";
        }

        foreach ((string name, DataValue value) in DataValues)
        {
            if ((value.Values?.Count ?? 0) == 0) continue;
            if (!Utils.MatchesBinEntry(key, name))
                continue;
            return $"{value.Values![rank]}";
        }

        for (var i = 0; i < EffectAmounts.Count; i++)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (EffectAmounts[i] == null) continue;
            var name = $"Effect{i + 1}Amount";
            if (name != key) continue;

            return $"{EffectAmounts[i][rank]}";
        }

        double? statValue = champion.GetStat(key);
        if (statValue.HasValue) return $"{Math.Round(statValue.Value)}";

        switch (key)
        {
            case "Hotkey":
                return Name.Last().ToString();
            case "Cooldown":
                return CooldownTime?[rank].ToString(CultureInfo.CurrentCulture);
            case "Cost":
                return Cost.Count > rank ? Cost[rank].ToString(CultureInfo.CurrentCulture) : "-1";
            case "MaxAmmo":
                return MaxAmmo.Count > rank ? MaxAmmo[rank].ToString(CultureInfo.CurrentCulture) : "-1";
            case "AmmoRechargeTime":
                return AmmoRechargeTime.Count > rank ? AmmoRechargeTime[rank].ToString(CultureInfo.CurrentCulture) : "-1";
            case "SpellModifierDescriptionAppend":
                return ""; // TODO figure this out
        }


        return null;
    }

    private string ResolveSpellText(string tooltip, ChampionInstance champion, int rank, SpellResolveMode mode)
    {
        string tt = CleanupToolTip(tooltip);

        MatchCollection references = Regex.Matches(tt, @"{{(.+?)}}");
        foreach (Match reference in references)
        {
            string? val = champion.Champ.Resolver.Get(reference.Groups[1].Value.Trim());
            if (val == null) continue;
            tt = tt.Replace(reference.Value, val);
        }

        MatchCollection pointers = Regex.Matches(tt, @"@[sS]pell\.(.+?):(.+?)(?:\*([\-\d]+)|)@");
        foreach (Match pointer in pointers)
        {
            Spell? spell = champion.Champ.Spells.FirstOrDefault(s => s.Name == pointer.Groups[1].Value)
                           ?? champion.Champ.AltSpells.FirstOrDefault(s => s.Name == pointer.Groups[1].Value);
            string? newValue = spell?.GetValue(pointer.Groups[2].Value, champion, rank, mode);
            if (newValue == null) continue;
            if (pointer.Groups[3].Success && double.TryParse(pointer.Groups[3].Value, out double mult))
            {
                tt = tt.Replace(pointer.Value,
                    double.TryParse(newValue, out double decValue)
                        ? $"{Math.Round(decValue * mult)}"
                        : $"({newValue} * {mult})");
                continue;
            }
            tt = tt.Replace(pointer.Value, newValue);
        }

        // example: @f1@
        // if replacement exists
        // then replace f1 with Replacements[0]
        // else replace with DataValues[1]

        // example: @f2@
        // if replacement exists
        // then replace f2 with Replacements[1]
        // else replace with Datavalues[2-len(Replacements)]

        // so in general
        // say we have @f<n>@
        // if Replacements.Count >= n: replace f<n> with Replacements[n-1]
        // if DataValues.Count > n-len(Replacements): replace f<n> with DataValues[n-len(Replacements)]
        // replace f<n> with EffectAmounts[n-len(Replacements)-len(DataValues]
        MatchCollection weirdF = Regex.Matches(tt, @"@([fF])([\d\.]+)(?:(\*\d+)|)@");
        foreach (Match match in weirdF)
        {
            // TODO: clean this up
            string? replacement = champion.Champ.Resolver.GetSpellReplacement(Name, $"{match.Groups[1].Value}{match.Groups[2].Value}");
            if (replacement != null)
            {
                tt = tt.Replace(match.Value, replacement);
                continue;
            }

            if(!int.TryParse(match.Groups[2].Value, out int n)) continue;
            if (Replacements.Count >= n)
            {
                string newVal = Replacements[n - 1];
                tt = tt.Replace($"{match.Groups[1].Value}{n}", newVal);
                continue;
            }
            if (DataValues.Count > 0 && DataValues.Count > n - Replacements.Count)
            {
                double newVal = DataValues.Values.ToArray()[n - Replacements.Count].Values[rank];
                if (match.Groups[3].Success && double.TryParse(match.Groups[3].Value, out double mult))
                    newVal *= mult;
                tt = tt.Replace(match.Value, newVal < 3 ? $"{Math.Round(newVal * 100)}%" : $"{newVal}");
                continue;
            }

            if (EffectAmounts.Count <= 0 || EffectAmounts.Count <= n - Replacements.Count - DataValues.Count) continue;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (EffectAmounts[n - Replacements.Count - DataValues.Count] == null)
            {
                if (EffectAmounts.Count < n) continue;
                double newVal = EffectAmounts[^n][rank];
                if (match.Groups[3].Success && double.TryParse(match.Groups[3].Value, out double mult))
                    newVal *= mult;
                tt = tt.Replace(match.Value, newVal < 3 ? $"{Math.Round(newVal * 100)}%" : $"{newVal}");
                continue;
            }

            double effVal = EffectAmounts[n - Replacements.Count - DataValues.Count][rank];
            if (match.Groups[3].Success && double.TryParse(match.Groups[3].Value, out double m))
                effVal *= m;
            tt = tt.Replace(match.Value, effVal < 3 ? $"{Math.Round(effVal * 100)}%" : $"{effVal}");
        }
        tt = tt.Replace(".0*", "*");

        MatchCollection matches = Regex.Matches(tt, $@"@(.+?)(?:\*([-\.0-9]+)%*|\.\d|)@");
        foreach (Match match in matches)
        {
            string? newValue = GetValue(match.Groups[1].Value, champion, rank, mode);
            if (newValue == null) continue;
            if (match.Groups[2].Success)
            {
                string mult = match.Groups[2].Value;
                if (mult[0] == '.') mult = "0" + mult;
                if (double.TryParse(mult, out double multDec) && double.TryParse(newValue, out double valDec))
                {
                    tt = tt.Replace(match.Value, $"{Math.Round(multDec * valDec)}");
                    continue;
                }
                tt = tt.Replace($"@{match.Groups[1].Value}*{mult}@", $"{mult} * {newValue}");
                continue;
            }
            tt = tt.Replace(match.Value, newValue);
        }

        tt = tt.Replace("@CharBonusPhysical@", "<bonus physical damage(WIP)>"); // TODO

        references = Regex.Matches(tt, @"{{(.+?)}}");
        if (references.Count <= 0) return tt;

        string donTt = tt;
        foreach (Match reference in references)
        {
            string? val = champion.Champ.Resolver.Get(reference.Groups[1].Value.Trim());
            if (val == null) continue;
            tt = tt.Replace(reference.Value, val);
        }

        return donTt != tt ? ResolveSpellText(tt, champion, rank, mode) : tt;
    }

    private static string CleanupToolTip(string tt)
    {
        // remove anything between brackets
        var r = new Regex(@"\(.*?\)");
        tt = r.Replace(tt, "");

        tt = tt.Replace("<br><br>", "\n");

        return RemoveToolTipSectionWithTag(tt, "rules", false);
    }

    private static string RemoveToolTipSectionWithTag(string tt, string tag, bool keepText)
    {
        Match match = Regex.Match(tt, $@"<{tag}>(.*?)<\/{tag}>");
        return match.Success ? tt.Replace(match.Value, keepText ? match.Groups[1].Value : "") : tt;
    }
}