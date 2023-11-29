using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class NumberCalculationPart : IGameCalculationPart
{
    private double Number;

    public double GetValue(CalculationContext context) => Number;

    public string String(CalculationContext context) => $"{Number}";

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mNumber", out JsonElement num))
            Number = num.GetDouble();
    }
    public CalculationPartType Type() => CalculationPartType.NumberCalculationPart;
}