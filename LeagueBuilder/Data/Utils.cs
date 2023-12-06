using System.Text;
using System.IO.Hashing;
using System.Text.Json;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Data;

public static class Utils
{
    public static bool MatchesBinEntry(string s, string binEntry) =>
        s.Equals(binEntry, StringComparison.CurrentCultureIgnoreCase)
        || $"{{{BinHash(s)}}}" == binEntry
        || $"{{{BinHash(s.ToLower())}}}" == binEntry;

    public static string BinHash(string s)
    {
        var hash = $"{Encoding.ASCII.GetBytes(s).Aggregate<byte, ulong>(0x811c9dc5, (current, b) => (current ^ b) * 0x01000193 % 0x100000000):x}";
        return hash.Length == 8 ? hash : '0' + hash;
    }

    public static string StringTableHash(string s) => Xxh64Trunc(s.ToLower());

    public static string Xxh64Trunc(string s)
    {
        var hash = new XxHash64();
        hash.Append(Encoding.ASCII.GetBytes(s));
        ulong h = hash.GetCurrentHashAsUInt64();
        h &= ((ulong)1 << 40) - 1;

        return $"{h:x}";
    }

    public static string Xxh64(string s)
    {
        var hash = new XxHash64();
        hash.Append(Encoding.ASCII.GetBytes(s));
        return $"{hash.GetCurrentHashAsUInt64():x}";
    }

    public static JsonElement? GetBinValue(string key, JsonDocument doc)
    {
        if (doc.RootElement.TryGetProperty(key, out JsonElement val)) return val;
        if (doc.RootElement.TryGetProperty($"{{{BinHash(key)}}}", out val)) return val;
        return null;
    }

    public static string StatToStringLong(StatType stat, StatFormulaType formulaType)
    {
        if (stat == StatType.AbilityPower) formulaType = StatFormulaType.Total;
        return $"{formulaType}{stat}";
    }

    public static string StatToStringShort(StatType stat, StatFormulaType formulaType)
    {
        string f = formulaType switch
        {
            StatFormulaType.Total => "t",
            StatFormulaType.Bonus => "b",
            _ => "",
        };
        string s = stat switch
        {
            StatType.AbilityPower => "AP",
            StatType.Attack => "AD",
            StatType.AttackSpeed => "AS",
            StatType.MagicResist => "MR",
            StatType.MoveSpeed => "MS",
            StatType.CooldownReduction => "CDR",
            StatType.AbilityHaste => "AH",
            StatType.MaxHealth => "HP",
            StatType.AttackRange => "AR",
            _ => stat.ToString()
        };
        return $"{f}{s}";
    }

    public static CalculationType CalculationTypeFromString(string s) => s switch
        {
            "IGameCalculation" => CalculationType.IGameCalculationType,
            "GameCalculation" => CalculationType.GameCalculationType,
            "GameCalculationConditional" => CalculationType.GameCalculationConditional,
            "GameCalculationModified" => CalculationType.GameCalculationModifiedType,
            _ => CalculationType.UnknownCalculation
        };

}