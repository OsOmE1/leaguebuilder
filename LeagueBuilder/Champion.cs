using System.Text.RegularExpressions;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder;

public class Champion
{
    public StringResolver Resolver;
    public string Name;
    public List<string> Tags;

    public AbilityResourceType AbilityResource;
    public double BaseAbilityResource;

    public Stats BaseStats;
    public Dictionary<StatType, double> PerLevel;
    public List<Spell> Spells;
    public List<Spell> AltSpells;

    private static List<SpellSlot> _slotOrder = [SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R, SpellSlot.Passive];
    public Champion(ApiChampion champion, StringResolver stringResolver)
    {
        Resolver = stringResolver;
        Name = champion.Character.Name;
        AbilityResource = (AbilityResourceType)champion.Character.PrimaryAbilityResource.ArType;
        BaseAbilityResource = champion.Character.PrimaryAbilityResource.ArBase;

        BaseStats = new Stats(champion.Character);
        PerLevel = new Dictionary<StatType, double>
        {
            { StatType.Attack, champion.Character.DamagePerLevel },
            { StatType.AttackSpeed, champion.Character.AttackSpeedPerLevel },
            { StatType.MaxHealth, champion.Character.HpPerLevel },
            { StatType.HealthRegenRate, champion.Character.HpRegenPerLevel },
            { StatType.Armor, champion.Character.ArmorPerLevel },
            { StatType.MagicResist, champion.Character.SpellBlockPerLevel },
            { StatType.ResourceRegenRate, champion.Character.PrimaryAbilityResource.ArRegenPerLevel }
        };
        Spells = new List<Spell>();
        for (var i = 0; i < champion.Spells.Count; i++)
        {
            ApiSpell rawSpell = champion.Spells[i];
            Match match = Regex.Match(rawSpell.MScriptName, @"^.*?(Q|E|W|R|Passive)$");

            SpellSlot slot = match.Groups[1].Value switch
            {
                "Q" => SpellSlot.Q,
                "E" => SpellSlot.E,
                "W" => SpellSlot.W,
                "R" => SpellSlot.R,
                "Passive" => SpellSlot.Passive,
                _ => i < _slotOrder.Count ? _slotOrder[i] : SpellSlot.Other
            };
            Spells.Add(new Spell(rawSpell, slot, stringResolver));
        }
        AltSpells = champion.AltSpells.Select(s => new Spell(s, SpellSlot.Other, stringResolver)).ToList();
    }

    public Spell? GetSpell(string name) => Spells.FirstOrDefault(s => s.Title == name || s.Name == name);
    public Spell? GetSpell(SpellSlot slot) => Spells.FirstOrDefault(s => s.Slot == slot);
    public Spell? GetAltSpell(SpellSlot slot) => AltSpells.FirstOrDefault(s => s.Slot == slot);

    public ChampionInstance GetInstance(int level) => new(this, level);
}