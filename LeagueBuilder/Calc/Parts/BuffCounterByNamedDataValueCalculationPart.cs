using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class BuffCounterByNamedDataValueCalculationPart : IGameCalculationPart
{
    private string BuffName;
    private DataValue? DataValue;

    public double GetValue(CalculationContext context)
    {
        double val = DataValue?.Values?[context.SpellRank] ?? -1;
        if (val > 0 && val < 0.01) val *= 100;
        return val;
    }

    public string String(CalculationContext context)
    {
        double val = GetValue(context);
        if (val > 0 && val < 1) val *= 100;
        return $"({Math.Round(val)}% {context.StringResolver.Get(BuffName) ?? BuffName})";
    }
    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mBuffName", out JsonElement buff))
            BuffName = buff.GetString()!;
        if (!element.TryGetProperty("mDataValue", out JsonElement dv)) return;
        DataValue = context.FindDataValue(dv.GetString()!);
        if (DataValue == null) throw new Exception("Could not find DataValue");
    }

    public CalculationPartType Type() => CalculationPartType.BuffCounterByNamedDataValueCalculationPart;
}