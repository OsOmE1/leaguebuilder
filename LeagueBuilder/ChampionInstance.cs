using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder;

public class ChampionInstance
{
    public Champion Champ;

    // shards
    public ShardType Offense;
    public ShardType Flex;
    public ShardType Defense;

    public Stats BaseStats;
    public Dictionary<StatType, double> PerLevel;
    public readonly List<Item> Items;
    public int Level;
    public List<object> Perks;

    private Dictionary<string,StatType> _statMap = new ()
    {
        { "abilitypower", StatType.AbilityPower },
        { "armor", StatType.Armor },
        { "attack", StatType.Attack },
        { "attackspeed", StatType.AttackSpeed },
        { "attackwinduptime", StatType.AttackWindupTime },
        { "magicresist", StatType.MagicResist },
        { "movementspeed", StatType.MoveSpeed },
        { "movespeed", StatType.MoveSpeed },
        { "critchance", StatType.CritChance },
        { "critdamage", StatType.CritDamage },
        { "cooldownreduction", StatType.CooldownReduction },
        { "abilityhaste", StatType.AbilityHaste },
        { "maxhealth", StatType.MaxHealth },
        { "currenthealth", StatType.CurrentHealth },
        { "percentmissinghealth", StatType.PercentMissingHealth },
        { "lifesteal", StatType.LifeSteal },
        { "omnivamp", StatType.OmniVamp },
        { "physicalvamp", StatType.PhysicalVamp },
        { "magicpenetrationflat", StatType.MagicPenetrationFlat },
        { "magicpenetrationpercent", StatType.MagicPenetrationPercent },
        { "magiclethality", StatType.MagicLethality },
        { "armorpenetrationflat", StatType.ArmorPenetrationFlat },
        { "armorpenetrationpercent", StatType.ArmorPenetrationPercent },
        { "physicallethality", StatType.PhysicalLethality },
        { "Tenacity", StatType.Tenacity },
        { "attackrange", StatType.AttackRange },
        { "healthregenrate", StatType.HealthRegenRate },
        { "Rrsourceregenrate", StatType.ResourceRegenRate },
        { "dodgechance", StatType.DodgeChance }
    };

    public ChampionInstance(Champion champ, int level)
    {
        Champ = champ;
        BaseStats = champ.BaseStats;
        PerLevel = champ.PerLevel;
        Level = level;

        Items = new List<Item>();
        Perks = new List<object>();

    }

    public void SetStatMod(StatModSlot slot, ShardType type)
    {
        switch (slot)
        {
            case StatModSlot.Offense:
                Offense = type;
                break;
            case StatModSlot.Flex:
                Flex = type;
                break;
            case StatModSlot.Defense:
                Defense = type;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
        }
    }

    public double GetResourceAmount(StatFormulaType type)
    {
        double baseValue = Champ.BaseAbilityResource;
        double bonusValue = Items.Sum(it => it.GetResourceFlat());

        return type switch
        {
            StatFormulaType.Base => baseValue,
            StatFormulaType.Bonus => bonusValue,
            StatFormulaType.Total => baseValue + bonusValue,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public double? GetStat(string statString)
    {
        statString = statString.ToLower().Replace(" ", "");
        var type = StatFormulaType.Base;
        if (statString.Contains("bonus"))
        {
            type = StatFormulaType.Bonus;
            statString = statString.Replace("bonus", "");
        }
        if (statString.Contains("total"))
        {
            type = StatFormulaType.Total;
            statString = statString.Replace("total", "");
        }

        if (!_statMap.TryGetValue(statString, out StatType stat)) return null;
        // there is no base ability power
        if (stat == StatType.AbilityPower) type = StatFormulaType.Total;

        return GetStat(stat, type);
    }

    public double GetStat(StatType stat, StatFormulaType type) => GetStat(stat, type, true);

    private double GetStat(StatType stat, StatFormulaType type, bool shardCalculation)
    {
        double baseValue = BaseStats.Values[stat];

        double bonusValue = 0;
        double bonusRatio = 0;
        foreach (Item it in Items)
        {
            bonusValue += it.GetStatFlat(stat);
            if (stat is StatType.ArmorPenetrationPercent or StatType.MagicPenetrationPercent)
                bonusRatio *= it.GetStatRatio(stat);
            else bonusRatio += it.GetStatRatio(stat);
        }

        if (shardCalculation)
            bonusValue += GetShardValue(stat);

        bonusValue += bonusRatio * baseValue; // Percent bonuses stack additively - credit: yariet :)

        if (stat is StatType.AbilityPower or StatType.MaxHealth or StatType.Attack)
            bonusValue += bonusRatio * bonusValue; // deathcap or wardstone edgecase

        double growth = 0;
        if (PerLevel.TryGetValue(stat, out double perLevel))
            growth = perLevel * (Level-1) * (0.7025+0.0175 * (Level-1)); // credit: yariet for this weird formula

        // stat formula does not matter for ap
        if (stat == StatType.AbilityPower) return bonusValue;

        return type switch
        {
            StatFormulaType.Base => baseValue + growth,
            StatFormulaType.Bonus => bonusValue,
            StatFormulaType.Total => baseValue + bonusValue + growth,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private double GetShardValue(StatType stat)
    {
        switch (stat) // this is very ugly but I don't feel like cleaning it up
        {
            case StatType.Attack or StatType.AbilityPower:
            {
                double adaptive = (Offense == ShardType.Adaptive ? 9 : 0) + (Flex == ShardType.Adaptive ? 9 : 0);
                double ad = GetStat(StatType.Attack, StatFormulaType.Bonus, false);
                double ap = GetStat(StatType.AbilityPower, StatFormulaType.Total, false);
                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                switch (stat)
                {
                    case StatType.Attack when ad > ap:
                        return adaptive * 0.6;
                    case StatType.AbilityPower when ap > ad:
                        return adaptive;
                }

                break;
            }
            case StatType.AttackSpeed:
                return Offense == ShardType.AttackSpeed ? 0.1 : 0;
            case StatType.AbilityHaste:
                return Offense == ShardType.AbilityHaste ? 8 : 0;
                break;
            case StatType.Armor:
                return (Flex == ShardType.Armor ? 6 : 0) + (Defense == ShardType.Armor ? 6 : 0);
            case StatType.MagicResist:
                return (Flex == ShardType.MagicResist ? 6 : 0) + (Defense == ShardType.MagicResist ? 6 : 0);
            case StatType.MaxHealth:
                return Defense == ShardType.Armor ? 15 + 125 * ((double)Level / 18F) : 0;
        }

        return 0;
    }

    public CalculationContext GetCalculationContext(SpellInstance instance, StringResolver fc) => new()
    {
        Champion = this,
        DataValues = instance.Spell.DataValues,
        EffectValues = instance.Spell.EffectAmounts,
        SpellRank = instance.Rank,
        Multiplier = 1.0,
        StringResolver = fc
    };

}