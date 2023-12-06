using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LeagueBuilder;

public class Config
{
    public string Patch { get; set; }
    public string Locale { get; set; }
    public Dictionary<string, string> KeyReplacements;
    public Dictionary<string, string> KeyLinks;
    public Dictionary<string, Dictionary<string,string>> SpellReplacements;
    public Dictionary<string, Dictionary<string, string>> BinReplacements;


    public static Config FromFile(string path)
    {
        string contents = File.ReadAllText(path);
        IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<Config>(contents);
    }
}