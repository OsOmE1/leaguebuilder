using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class EffectValueCalculationPart : IGameCalculationPart
{
    private int EffectIndex;

    public double GetValue(CalculationContext context)
    {
        if (context.EffectValues.Count == 0) return 0;
        return context.EffectValues[EffectIndex][context.SpellRank];
    }

    public string String(CalculationContext context)
    {
        double val = GetValue(context);
        if (val is > 0 and < 3) val *= 100;
        return $"{Math.Round(val)}";
    }

    public void FromJson(JsonElement element, ParseContext context)
    {
       if (element.TryGetProperty("mEffectIndex", out JsonElement i))
           EffectIndex = i.GetInt32() - 1;
    }

    public CalculationPartType Type() => CalculationPartType.EffectValueCalculationPart;
}