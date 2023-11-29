using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class ByCharLevelInterpolationCalculationPart : IGameCalculationPart
{
    private double StartValue;
    private double EndValue;

    public double GetValue(CalculationContext context) =>
        StartValue + (EndValue-StartValue)*(((double)context.Champion.Level)/18F);

    public string String(CalculationContext context) => $"{GetValue(context)}";

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mStartValue", out JsonElement start))
            StartValue = start.GetDouble();
        if (element.TryGetProperty("mEndValue", out JsonElement end))
            EndValue = end.GetDouble();
    }

    public CalculationPartType Type() => CalculationPartType.ByCharLevelInterpolationCalculationPart;
}