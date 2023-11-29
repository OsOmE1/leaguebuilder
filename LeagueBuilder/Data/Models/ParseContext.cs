namespace LeagueBuilder.Data.Models;

public class ParseContext(ApiSpell rawSpell, Dictionary<string, DataValue> dataValues)
{
    public ApiSpell RawSpell = rawSpell;
    public Dictionary<string, DataValue> DataValues = dataValues;

    public DataValue? FindDataValue(string key) => DataValues
        .FirstOrDefault(d =>
            string.Equals(d.Key, key, StringComparison.CurrentCultureIgnoreCase) ||
            $"{{{Utils.BinHash(d.Key)}}}" == key ||
            $"{{{Utils.BinHash(d.Key.ToLower())}}}" == key).Value;
}