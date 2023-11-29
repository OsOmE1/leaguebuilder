namespace LeagueBuilder.Data.Models;

public class Stats(CharacterRecord record)
{
    public AbilityResourceType ResourceType = (AbilityResourceType)record.PrimaryAbilityResource.ArType;
    public double ResourceValue = record.PrimaryAbilityResource.ArBase;
    public double ResourceValuePerLevel = record.PrimaryAbilityResource.ArPerLevel;

    public Dictionary<StatType, double> Values = new()
    {
        { StatType.AbilityPower, 0 },
        { StatType.Armor, record.BaseArmor },
        { StatType.Attack, record.BaseDamage },
        { StatType.AttackSpeed, record.AttackSpeed },
        { StatType.AttackWindupTime, record.BasicAttack.MAttackCastTime },
        { StatType.MagicResist, record.BaseSpellBlock },
        { StatType.MoveSpeed, record.BaseMoveSpeed },
        { StatType.CritChance, 0 },
        { StatType.CritDamage, 0 },
        { StatType.CooldownReduction, 0 },
        { StatType.AbilityHaste, 0 },
        { StatType.MaxHealth, record.BaseHP },
        { StatType.CurrentHealth, record.BaseHP },
        { StatType.PercentMissingHealth, 0 },
        { StatType.LifeSteal, 0 },
        { StatType.OmniVamp, 0 },
        { StatType.PhysicalVamp, 0 },
        { StatType.MagicPenetrationFlat, 0 },
        { StatType.MagicPenetrationPercent, 0 },
        { StatType.BonusMagicPenetrationPercent, 0 },
        { StatType.MagicLethality, 0 },
        { StatType.ArmorPenetrationFlat, 0 },
        { StatType.ArmorPenetrationPercent, 0 },
        { StatType.BonusArmorPenetrationPercent, 0 },
        { StatType.PhysicalLethality, 0 },
        { StatType.Tenacity, 0 },
        { StatType.AttackRange, record.AttackRange },
        { StatType.HealthRegenRate, record.BaseStaticHPRegen },
        { StatType.ResourceRegenRate, record.PrimaryAbilityResource.ArBaseStaticRegen },
        { StatType.DodgeChance, 0 },
    };
}