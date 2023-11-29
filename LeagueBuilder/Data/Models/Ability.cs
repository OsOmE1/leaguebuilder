using System.Text.Json.Serialization;

namespace LeagueBuilder.Data.Models;

public class Ability
{
    [JsonPropertyName("mRootSpell")]
    public string MRootSpell { get; set; }
    [JsonPropertyName("mChildSpells")]
    public List<string> MChildSpells { get; set; }
    [JsonPropertyName("mName")]
    public string MName { get; set; }
    [JsonPropertyName("mType")]
    public int MType { get; set; }
    [JsonPropertyName("__type")]
    public string Type { get; set; }

    public Ability() {}
}