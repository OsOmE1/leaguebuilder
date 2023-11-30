using System.Text.Json;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class BuffCounterByCoefficientCalculationPart  : IGameCalculationPart
{
    private double Coefficient;
    private string BuffName;

    public double GetValue(CalculationContext context)
    {
        double val = Coefficient;
        if (val is > 0 and < 0.01) val *= 100;
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
        if (element.TryGetProperty("mCoefficient", out JsonElement coef))
            Coefficient = coef.GetDouble();
        if (element.TryGetProperty("mBuffName", out JsonElement buff))
            BuffName = buff.GetString();
    }

    public CalculationPartType Type() => CalculationPartType.BuffCounterByCoefficientCalculationPart;
}