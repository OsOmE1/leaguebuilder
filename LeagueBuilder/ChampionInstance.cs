﻿using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder;

public class ChampionInstance
{
    public Champion Champ;
    public Stats BaseStats;
    public Dictionary<StatType, double> PerLevel;
    public List<object> Items;
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

        Items = new List<object>();
        Perks = new List<object>();

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

    public double GetStat(StatType stat, StatFormulaType type)
    {
        double baseValue = BaseStats.Values[stat];
        if (PerLevel.TryGetValue(stat, out double perLevel)) baseValue += perLevel * Level;

        double bonusValue = 0;
        foreach (object it in Items) continue; // TODO: implement items
        foreach (object it in Items) continue; // TODO: implement runes

        return type switch
        {
            StatFormulaType.Base => baseValue,
            StatFormulaType.Bonus => bonusValue,
            StatFormulaType.Total => baseValue + bonusValue,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
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