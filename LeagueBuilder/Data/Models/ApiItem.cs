using System.Text.Json;

namespace LeagueBuilder.Data.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Element
{
    public string type { get; set; }
    public string __type { get; set; }
}

public class MItemDataAvailability
{
    public bool mInStore { get; set; }
    public string __type { get; set; }
}

public class MItemDataClient
{
    public MTooltipData mTooltipData { get; set; }
    public string mDescription { get; set; }
    public string mDynamicTooltip { get; set; }
    public string mShopTooltip { get; set; }
    public string inventoryIcon { get; set; }
    public int epicness { get; set; }
    public string __type { get; set; }
}

public class ItemMLists
{
    public ItemStats Stats { get; set; }
}

public class MLocKeys
{
    public string keyName { get; set; }
    public string keyTooltipInventory { get; set; }
    public string keyTooltipExternal { get; set; }
    public string keyColloquialism { get; set; }
    public string keyUniqueGroup { get; set; }
    public string keyBrief { get; set; }
    public string keyTooltip { get; set; }
}

public class MTooltipData
{
    public string mObjectName { get; set; }
    public string mFormat { get; set; }
    public MLocKeys mLocKeys { get; set; }
    public ItemMLists mLists { get; set; }
    public string __type { get; set; }
}

public class ApiItem
{
    public string mDisplayName { get; set; }
    public string mRequiredSpellName { get; set; }
    public int itemID { get; set; }
    public int maxStack { get; set; }
    public List<string> mItemGroups { get; set; }
    public int price { get; set; }
    public bool mItemCalloutSpectator { get; set; }
    public bool mCanBeSold { get; set; }
    public int epicness { get; set; }

    public double mAbilityHasteMod { get; set; }
    public double mFlatHPPoolMod { get; set; }
    public double mFlatMagicDamageMod { get; set; }
    public double PercentOmnivampMod { get; set; }
    public double mPercentBaseHPRegenMod { get; set; }
    public double mFlatArmorMod { get; set; }
    public double flatMPPoolMod { get; set; }
    public double mPercentMovementSpeedMod { get; set; }
    public double mFlatPhysicalDamageMod { get; set; }
    public double mPercentHealingAmountMod { get; set; }
    public double percentBaseMPRegenMod { get; set; }
    public double mFlatSpellBlockMod { get; set; }
    public double mFlatCritChanceMod { get; set; }
    public double mFlatMagicPenetrationMod { get; set; }
    public double mFlatMovementSpeedMod { get; set; }
    public double mFlatHPRegenMod { get; set; }
    public double mPercentArmorPenetrationMod { get; set; }
    public double mPercentAttackSpeedMod { get; set; }
    public double mPercentLifeStealMod { get; set; }
    public double mFlatCritDamageMod { get; set; }
    public double mPercentMagicPenetrationMod { get; set; }
    public double flatMPRegenMod { get; set; }
    public double mPercentTenacityItemMod { get; set; }
    public double mPercentSlowResistMod { get; set; }

    public JsonDocument? mItemCalculations { get; set; }
    public List<RawDataValue>? mDataValues { get; set; }

    public List<string> mCategories { get; set; }
    public MItemDataAvailability mItemDataAvailability { get; set; }
    public List<int> mItemAttributes { get; set; }
    public MItemDataClient mItemDataClient { get; set; }
    public string __type { get; set; }
}

public class ItemStats
{
    public List<Element> elements { get; set; }
    public string __type { get; set; }
}

