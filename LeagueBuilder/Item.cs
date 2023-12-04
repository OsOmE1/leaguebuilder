using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder;

public class Item
{
    public ApiItem Raw;
    public string Name;
    public string Title;
    public int Id;
    public int Epicness;

    public List<ItemCategory> Categories;
    public List<ItemStat> ItemStats;

    public double AbilityHaste;
    public double Lethality;
    public double FlatHp;
    public double PercentHp;
    public double FlatAbilityPower;
    public double PercentAbilityPower;
    public double PercentOmnivamp;
    public double PercentBaseHpRegen;
    public double FlatArmor;
    public double FlatMana;
    public double PercentMovementSpeed;
    public double FlatPhysicalDamage;
    public double PercentPhysicalDamage;
    public double PercentHealingAmount;
    public double PercentBaseManaRegen;
    public double FlatMagicResist;
    public double FlatCritChance;
    public double FlatMagicPenetration;
    public double FlatMovementSpeed;
    public double FlatHpRegen;
    public double PercentArmorPenetration;
    public double PercentAttackSpeed;
    public double PercentLifeSteal;
    public double FlatCritDamage;
    public double PercentMagicPenetration;
    public double FlatManaRegen;
    public double PercentTenacity;
    public double PercentSlowResist;

    private static Dictionary<StatType, ItemStat> _statMap = new()
    {
        { StatType.AbilityPower, ItemStat.AbilityPower },
        { StatType.MaxHealth, ItemStat.Health },
        { StatType.AbilityHaste, ItemStat.AbilityHaste },
        { StatType.OmniVamp, ItemStat.Omnivamp },
        { StatType.Armor, ItemStat.Armor },
        { StatType.HealthRegenRate, ItemStat.BaseHealthRegen },
        { StatType.Attack, ItemStat.AttackDamage },
        // { StatType.?, ItemStat.HealShieldPower },
        { StatType.MagicResist, ItemStat.MagicResist },
        { StatType.PhysicalLethality, ItemStat.Lethality },
        { StatType.CritChance, ItemStat.CritChance },
        { StatType.MagicPenetrationFlat, ItemStat.MagicPen },
        { StatType.MoveSpeed, ItemStat.MoveSpeed },
        { StatType.ArmorPenetrationFlat, ItemStat.Lethality },
        { StatType.ArmorPenetrationPercent, ItemStat.ArmorPenPercent },
        { StatType.AttackSpeed, ItemStat.AttackSpeed },
        { StatType.LifeSteal, ItemStat.LifeSteal },
        { StatType.CritDamage, ItemStat.CritDamageFlat },
        { StatType.MagicPenetrationPercent, ItemStat.MagicPenPercent },
        { StatType.Tenacity, ItemStat.Tenacity }
        // { StatType.?, ItemStat.GoldPer10 },
    };
    public Item(ApiItem item, StringResolver stringResolver)
    {
        Raw = item;
        Name = item.mDisplayName;
        Id = item.itemID;
        Epicness = item.epicness;
        Title = stringResolver.Get(item.mItemDataClient?.mTooltipData?.mLocKeys?.keyName ?? "") ?? "";

        Categories = item.mCategories?.Select(s => (ItemCategory)Enum.Parse(typeof(ItemCategory), s)).ToList() ?? [];
        ItemStats = item.mItemDataClient?.mTooltipData?.mLists?.Stats?.elements
            ?.Select(s => (ItemStat)Enum.Parse(typeof(ItemStat), s.type)).ToList() ?? [];

        // BonusStatMod or deathcap
        PercentAbilityPower = (item.mDataValues?.FirstOrDefault(r => r.MName == "APAmp")?.Value ??
                               item.mDataValues?.FirstOrDefault(r => r.MName == "BonusStatMod")?.Value) ?? 0;

        PercentPhysicalDamage = item.mDataValues?.FirstOrDefault(r => r.MName == "BonusStatMod")?.Value ?? 0;
        PercentHp = item.mDataValues?.FirstOrDefault(r => r.MName == "BonusStatMod")?.Value ?? 0;

        Lethality = item.mDataValues?.FirstOrDefault(r => r.MName == "LethalityAmount")?.Value ?? 0;

        AbilityHaste = item.mAbilityHasteMod;
        FlatHp = item.mFlatHPPoolMod;
        FlatAbilityPower = item.mFlatMagicDamageMod;
        PercentOmnivamp = item.PercentOmnivampMod;
        PercentBaseHpRegen = item.mPercentBaseHPRegenMod;
        FlatArmor = item.mFlatArmorMod;
        FlatMana = item.flatMPPoolMod;
        PercentMovementSpeed = item.mPercentMovementSpeedMod;
        FlatPhysicalDamage = item.mFlatPhysicalDamageMod;
        PercentHealingAmount = item.mPercentHealingAmountMod;
        PercentBaseManaRegen = item.percentBaseMPRegenMod;
        FlatMagicResist = item.mFlatSpellBlockMod;
        FlatCritChance = item.mFlatCritChanceMod;
        FlatMagicPenetration = item.mFlatMagicPenetrationMod;
        FlatMovementSpeed = item.mFlatMovementSpeedMod;
        FlatHpRegen = item.mFlatHPRegenMod;
        PercentArmorPenetration = item.mPercentArmorPenetrationMod;
        PercentAttackSpeed = item.mPercentAttackSpeedMod;
        PercentLifeSteal = item.mPercentLifeStealMod;
        FlatCritDamage = item.mFlatCritDamageMod;
        PercentMagicPenetration = item.mPercentMagicPenetrationMod;
        FlatManaRegen = item.flatMPRegenMod;
        PercentTenacity = item.mPercentTenacityItemMod;
        PercentSlowResist = item.mPercentSlowResistMod;
    }

    public double GetResourceFlat() => FlatMana;
    public double GetResourceRegenFlat() => FlatManaRegen;
    public double GetResourceRegenRatio() => FlatManaRegen / 100;

    public double GetStatFlat(StatType stat)
    {
        if (!_statMap.TryGetValue(stat, out ItemStat itemStat)) return 0;
        return itemStat switch
        {
            ItemStat.AbilityPower => FlatAbilityPower,
            ItemStat.Health => FlatHp,
            ItemStat.AbilityHaste => AbilityHaste,
            ItemStat.Armor => FlatArmor,
            ItemStat.BaseHealthRegen => FlatHpRegen,
            ItemStat.AttackDamage => FlatPhysicalDamage,
            ItemStat.MagicResist => FlatMagicResist,
            ItemStat.Lethality => Lethality,
            ItemStat.CritChance => FlatCritChance,
            ItemStat.MagicPen => FlatMagicPenetration,
            ItemStat.MoveSpeed => FlatMovementSpeed,
            ItemStat.CritDamageFlat => FlatCritDamage,
            _ => 0
        };
    }

    public double GetStatRatio(StatType stat)
    {
        if (!_statMap.TryGetValue(stat, out ItemStat itemStat)) return 0;
        return itemStat switch
        {
            ItemStat.Omnivamp => PercentOmnivamp / 100,
            ItemStat.BaseHealthRegen => PercentBaseHpRegen / 100,
            ItemStat.HealShieldPower => PercentHealingAmount / 100,
            ItemStat.MoveSpeed => PercentMovementSpeed / 100,
            ItemStat.ArmorPenPercent => PercentArmorPenetration / 100,
            ItemStat.AttackSpeed => PercentAttackSpeed / 100,
            ItemStat.LifeSteal => PercentLifeSteal / 100,
            ItemStat.MagicPenPercent => PercentMagicPenetration,
            ItemStat.Tenacity => PercentTenacity / 100,
            ItemStat.AbilityPower => PercentAbilityPower,
            _ => 0
        };
    }
}