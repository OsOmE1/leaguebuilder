using System.Text.Json.Serialization;

namespace LeagueBuilder.Data.Models;

public class ApiAbilityResource
{
	[JsonPropertyName("arType")]
	public int ArType { get; set; }
	[JsonPropertyName("arBase")]
	public double ArBase { get; set; }
	[JsonPropertyName("arPerLevel")]
	public double ArPerLevel { get; set; }
	[JsonPropertyName("arBaseStaticRegen")]
	public double ArBaseStaticRegen { get; set; }
	[JsonPropertyName("arIncrements")]
	public double ArIncrements { get; set; }
	[JsonPropertyName("arMaxSegments")]
	public int ArMaxSegments { get; set; }
	[JsonPropertyName("arAllowMaxValueToBeOverridden")]
	public bool ArAllowMaxValueToBeOverridden { get; set; }
	[JsonPropertyName("arRegenPerLevel")]
	public double ArRegenPerLevel { get; set; }
	[JsonPropertyName("__type")]
	public string Type { get; set; }

	public ApiAbilityResource()
	{

	}
}

public class ApiAttack
{
	[JsonPropertyName("mAttackTotalTime")]
	public double MAttackTotalTime { get; set; }
	[JsonPropertyName("mAttackCastTime")]
	public double MAttackCastTime { get; set; }
	[JsonPropertyName("mAttackDelayCastOffsetPercentAttackSpeedRatio")]
	public double MAttackDelayCastOffsetPercentAttackSpeedRatio { get; set; }
	[JsonPropertyName("mAttackName")]
	public string MAttackName { get; set; }
	[JsonPropertyName("__type")]
	public string Type { get; set; }

	public ApiAttack()
	{

	}
}

public class CharacterToolData
{
	[JsonPropertyName("searchTags")]
	public string SearchTags { get; set; }
	[JsonPropertyName("searchTagsSecondary")]
	public string SearchTagsSecondary { get; set; }
	[JsonPropertyName("championId")]
	public int ChampionId { get; set; }
	[JsonPropertyName("roles")]
	public string Roles { get; set; }
	[JsonPropertyName("description")]
	public string Description { get; set; }
	[JsonPropertyName("classification")]
	public string Classification { get; set; }
	[JsonPropertyName("__type")]
	public string Type { get; set; }

	public CharacterToolData()
	{

	}
}

public class PerkReplacement
{
	[JsonPropertyName("mReplaceTarget")]
	public string MReplaceTarget { get; set; }
	[JsonPropertyName("mReplaceWith")]
	public string MReplaceWith { get; set; }
	[JsonPropertyName("__type")]
	public string Type { get; set; }

	public PerkReplacement()
	{

	}
}

public class PerkReplacements
{
	[JsonPropertyName("mReplacements")]
	public List<PerkReplacement> MReplacements { get; set; }
	[JsonPropertyName("__type")]
	public string Type { get; set; }

	public PerkReplacements()
	{

	}
}

public class PassiveBuff
{
	[JsonPropertyName("{bd3c31e4}")]
	public string Bd3C31E4 { get; set; }
	[JsonPropertyName("__type")]
	public string Type { get; set; }

	public PassiveBuff() { }
}

public class CharacterRecord
{
	[JsonPropertyName("mCharacterName")]
	public string MCharacterName { get; set; }
	[JsonPropertyName("baseHP")]
	public double BaseHP { get; set; }
	[JsonPropertyName("hpPerLevel")]
	public double HpPerLevel { get; set; }
	[JsonPropertyName("baseStaticHPRegen")]
	public double BaseStaticHPRegen { get; set; }
	[JsonPropertyName("hpRegenPerLevel")]
	public double HpRegenPerLevel { get; set; }
	[JsonPropertyName("healthBarHeight")]
	public double HealthBarHeight { get; set; }
	[JsonPropertyName("primaryAbilityResource")]
	public ApiAbilityResource PrimaryAbilityResource { get; set; }
	[JsonPropertyName("secondaryAbilityResource")]
	public ApiAbilityResource SecondaryAbilityResource { get; set; }
	[JsonPropertyName("baseDamage")]
	public double BaseDamage { get; set; }
	[JsonPropertyName("damagePerLevel")]
	public double DamagePerLevel { get; set; }
	[JsonPropertyName("baseArmor")]
	public double BaseArmor { get; set; }
	[JsonPropertyName("armorPerLevel")]
	public double ArmorPerLevel { get; set; }
	[JsonPropertyName("baseSpellBlock")]
	public double BaseSpellBlock { get; set; }
	[JsonPropertyName("spellBlockPerLevel")]
	public double SpellBlockPerLevel { get; set; }
	[JsonPropertyName("baseMoveSpeed")]
	public double BaseMoveSpeed { get; set; }
	[JsonPropertyName("attackRange")]
	public double AttackRange { get; set; }
	[JsonPropertyName("attackSpeed")]
	public double AttackSpeed { get; set; }
	[JsonPropertyName("attackSpeedRatio")]
	public double AttackSpeedRatio { get; set; }
	[JsonPropertyName("attackSpeedPerLevel")]
	public double AttackSpeedPerLevel { get; set; }
	[JsonPropertyName("basicAttack")]
	public ApiAttack BasicAttack { get; set; }
	[JsonPropertyName("extraAttacks")]
	public List<ApiAttack> ExtraAttacks { get; set; }
	[JsonPropertyName("critAttacks")]
	public List<ApiAttack> CritAttacks { get; set; }
	[JsonPropertyName("spellNames")]
	public List<string> SpellNames { get; set; }
	[JsonPropertyName("mAbilities")]
	public List<string>? MAbilities { get; set; }
	[JsonPropertyName("passiveLuaName")]
	public string PassiveLuaName { get; set; }
	[JsonPropertyName("passiveSpell")]
	public string PassiveSpell { get; set; }
	[JsonPropertyName("passive1IconName")]
	public string Passive1IconName { get; set; }
	[JsonPropertyName("name")]
	public string Name { get; set; }
	[JsonPropertyName("selectionHeight")]
	public double SelectionHeight { get; set; }
	[JsonPropertyName("selectionRadius")]
	public double SelectionRadius { get; set; }
	[JsonPropertyName("pathfindingCollisionRadius")]
	public double PathfindingCollisionRadius { get; set; }
	[JsonPropertyName("unitTagsString")]
	public string UnitTagsString { get; set; }
	[JsonPropertyName("characterToolData")]
	public CharacterToolData CharacterToolData { get; set; }
	[JsonPropertyName("platformEnabled")]
	public bool PlatformEnabled { get; set; }
	[JsonPropertyName("useRiotRelationships")]
	public bool UseRiotRelationships { get; set; }
	[JsonPropertyName("purchaseIdentities")]
	public List<string> PurchaseIdentities { get; set; }
	[JsonPropertyName("mPreferredPerkStyle")]
	public string MPreferredPerkStyle { get; set; }
	[JsonPropertyName("mPerkReplacements")]
	public PerkReplacements MPerkReplacements { get; set; }
	[JsonPropertyName("mCharacterPassiveSpell")]
	public string MCharacterPassiveSpell { get; set; }
	[JsonPropertyName("mCharacterPassiveBuffs")]
	public List<PassiveBuff> MCharacterPassiveBuffs { get; set; }
	[JsonPropertyName("__type")]
	public string Type { get; set; }
	public CharacterRecord()
	{

	}
}