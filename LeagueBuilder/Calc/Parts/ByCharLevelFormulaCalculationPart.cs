using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class ByCharLevelFormulaCalculationPart : IGameCalculationPart
{
    private List<double> Values;

    public double GetValue(CalculationContext context) => Values[context.Champion.Level];

    public string String(CalculationContext context) => $"{GetValue(context)}";

    public void FromJson(JsonElement element, ParseContext context) =>
        Values = element.GetProperty("mValues").EnumerateArray().Select(v => v.GetDouble()).ToList();

    public CalculationPartType Type() => CalculationPartType.ByCharLevelFormulaCalculationPart;
}