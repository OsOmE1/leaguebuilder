using System.Text.Json.Serialization;

namespace LeagueBuilder.Data.Models;

public class LocationKey
{
    [JsonPropertyName("keyName")]
    public string? KeyName { get; set; }
    [JsonPropertyName("keyCost")]
    public string? KeyCost { get; set; }
    [JsonPropertyName("keyTooltip")]
    public string? KeyTooltip { get; set; }
    [JsonPropertyName("keySummary")]
    public string? KeySummary { get; set; }
    [JsonPropertyName("keyTooltipExtendedBelowLine")]
    public string? KeyTooltipExtendedBelowLine { get; set; }

    public LocationKey() {}
}

public class LevelUpElement
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("multiplier")]
    public double Multiplier { get; set; }
    [JsonPropertyName("nameOverride")]
    public string NameOverride { get; set; }
    [JsonPropertyName("Style")]
    public int Style { get; set; }
    [JsonPropertyName("__type")]
    public string Type1 { get; set; }
    [JsonPropertyName("typeIndex")]
    public int TypeIndex { get; set; }

    public LevelUpElement() {}
}

public class LevelUp
{
    [JsonPropertyName("levelCount")]
    public int LevelCount { get; set; }
    [JsonPropertyName("elements")]
    public List<LevelUpElement> Elements { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public LevelUp() {}
}

public class MLists
{
    [JsonPropertyName("LevelUp")]
    public LevelUp LevelUp { get; set; }
    public MLists() {}
}

public class TooltipData
{
    [JsonPropertyName("mObjectName")]
    public string MObjectName { get; set; }
    [JsonPropertyName("mFormat")]
    public string MFormat { get; set; }
    [JsonPropertyName("mLocKeys")]
    public LocationKey? MLocKeys { get; set; }
    [JsonPropertyName("mLists")]
    public MLists MLists { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public TooltipData() {}
}

public class OverrideBaseRange
{
    [JsonPropertyName("mPerLevelValues")]
    public List<double> MPerLevelValues { get; set; }
    [JsonPropertyName("mValueType")]
    public int MValueType { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public OverrideBaseRange() { }
}

public class EndLocator
{
    [JsonPropertyName("basePosition")]
    public int BasePosition { get; set; }
    [JsonPropertyName("orientationType")]
    public int OrientationType { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public EndLocator() { }
}

public class LineWidth
{
    [JsonPropertyName("mPerLevelValues")]
    public List<double> MPerLevelValues { get; set; }
    [JsonPropertyName("mValueType")]
    public int MValueType { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public LineWidth() { }
}


public class TargeterDefinitions
{
    [JsonPropertyName("useCasterBoundingBox")]
    public bool UseCasterBoundingBox { get; set; }
    [JsonPropertyName("overrideBaseRange")]
    public OverrideBaseRange OverrideBaseRange { get; set; }
    [JsonPropertyName("endLocator")]
    public EndLocator EndLocator { get; set; }
    [JsonPropertyName("alwaysDraw")]
    public bool AlwaysDraw { get; set; }
    [JsonPropertyName("minimumDisplayedRange")]
    public double MinimumDisplayedRange { get; set; }
    [JsonPropertyName("lineWidth")]
    public LineWidth LineWidth { get; set; }
    [JsonPropertyName("textureBaseOverrideName")]
    public string TextureBaseOverrideName { get; set; }
    [JsonPropertyName("textureTargetOverrideName")]
    public string TextureTargetOverrideName { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public TargeterDefinitions() { }
}

public class ClientData
{
    [JsonPropertyName("mTooltipData")]
    public TooltipData? MTooltipData { get; set; }
    [JsonPropertyName("mTargeterDefinitions")]
    public List<TargeterDefinitions> MTargeterDefinitions { get; set; }

    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public ClientData() {}
}