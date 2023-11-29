using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class NamedDataValueCalculationPart : IGameCalculationPart
{
    private string BuffName;
    private DataValue? DataValue;

    public double GetValue(CalculationContext context) => DataValue?.Values[context.SpellRank] ?? -1;

    public string String(CalculationContext context)
    {
        double val = GetValue(context);
        if (val is > 0 and < 3) val *= 100;
        return $"{Math.Round(val)}";
    }

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (!element.TryGetProperty("mDataValue", out JsonElement dv)) return;
        DataValue = context.FindDataValue(dv.GetString()!);
    }

    public CalculationPartType Type() => CalculationPartType.NamedDataValueCalculationPart;
}