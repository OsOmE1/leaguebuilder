using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeagueBuilder.Data.Models;

public class RequiredUnitTags
{
    [JsonPropertyName("mTagList")]
    public string MTagList { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public RequiredUnitTags() {}
}

public class EffectAmount
{
    [JsonPropertyName("value")]
    public List<double> Value { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public EffectAmount() {}
}

public class RawDataValue
{
    [JsonPropertyName("mName")]
    public string MName { get; set; }
    [JsonPropertyName("mValues")]
    public List<double> Values { get; set; }
    [JsonPropertyName("mValue")]
    public double Value { get; set; }
    [JsonPropertyName("mBaseP")]
    public double MBaseP { get; set; }
    [JsonPropertyName("mFormula")]
    public string MFormula { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public RawDataValue() {}
}

public class SpellContainer
{
    [JsonPropertyName("flags")]
    public int Flags { get; set; }
    [JsonPropertyName("mAlternateName")]
    public string MAlternateName { get; set; }
    [JsonPropertyName("mAffectsTypeFlags")]
    public int MAffectsTypeFlags { get; set; }
    [JsonPropertyName("mRequiredUnitTags")]
    public RequiredUnitTags MRequiredUnitTags { get; set; }
    [JsonPropertyName("mSpellTags")]
    public List<string> MSpellTags { get; set; }
    [JsonPropertyName("mEffectAmount")]
    public List<EffectAmount>? MEffectAmount { get; set; }
    [JsonPropertyName("mDataValues")]
    public List<RawDataValue>? MDataValues { get; set; }
    [JsonPropertyName("mSpellCalculations")]
    public JsonDocument? MSpellCalculations { get; set; } // TODO Check
    [JsonPropertyName("mCoefficient")]
    public double MCoefficient { get; set; }
    [JsonPropertyName("mAnimationName")]
    public string MAnimationName { get; set; }
    [JsonPropertyName("mImgIconName")]
    public List<string> MImgIconName { get; set; }
    [JsonPropertyName("mCastTime")]
    public double MCastTime { get; set; }
    [JsonPropertyName("cooldownTime")]
    public List<double> CooldownTime { get; set; }
    [JsonPropertyName("mMaxAmmo")]
    public List<int>? MaxAmmo { get; set; }
    [JsonPropertyName("mAmmoRechargeTime")]
    public List<double>? AmmoRechargeTime { get; set; }
    [JsonPropertyName("delayCastOffsetPercent")]
    public double DelayCastOffsetPercent { get; set; }
    [JsonPropertyName("delayTotalTimePercent")]
    public double DelayTotalTimePercent { get; set; }
    [JsonPropertyName("mCantCancelWhileWindingUp")]
    public bool MCantCancelWhileWindingUp { get; set; }
    [JsonPropertyName("mChannelIsInterruptedByDisables")]
    public bool MChannelIsInterruptedByDisables { get; set; }
    [JsonPropertyName("mCanMoveWhileChanneling")]
    public bool MCanMoveWhileChanneling { get; set; }
    [JsonPropertyName("useAnimatorFramerate")]
    public bool UseAnimatorFramerate { get; set; }
    [JsonPropertyName("castRange")]
    public List<double> CastRange { get; set; }
    [JsonPropertyName("castRadius")]
    public List<double> CastRadius { get; set; }
    [JsonPropertyName("castConeDistance")]
    public double CastConeDistance { get; set; }
    [JsonPropertyName("castFrame")]
    public double CastFrame { get; set; }
    [JsonPropertyName("missileSpeed")]
    public double MissileSpeed { get; set; }
    [JsonPropertyName("mana")]
    public List<double> Mana { get; set; }
    [JsonPropertyName("bHaveHitBone")]
    public bool BHaveHitBone { get; set; }
    [JsonPropertyName("mHitBoneName")]
    public string MHitBoneName { get; set; }
    [JsonPropertyName("mTargetingTypeData")]
    public JsonProperty MTargetingTypeData { get; set; }
    [JsonPropertyName("mClientData")]
    public ClientData? MClientData { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public SpellContainer() {}
}

public class SpellBuff
{
    [JsonPropertyName("mDescription")]
    public string MDescription { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public SpellBuff() { }
}

public class ApiSpell
{
    [JsonPropertyName("mScriptName")]
    public string MScriptName { get; set; }
    [JsonPropertyName("mSpell")]
    public SpellContainer MSpell { get; set; }
    [JsonPropertyName("mBuff")]
    public SpellBuff MBuff { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }
}