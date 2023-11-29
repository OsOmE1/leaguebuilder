using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class SumOfSubPartsCalculationPart : IGameCalculationPart
{
    private List<IGameCalculationPart> Parts;

    public double GetValue(CalculationContext context) => Parts.Sum(p => p.GetValue(context));

    public string String(CalculationContext context) => $"({string.Join(" + ", Parts.Select(p => p.String(context)))})";

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mSubpart", out JsonElement subPart))
        {
            if (element.TryGetProperty("__type", out JsonElement t) &&
                GameCalculationPart.FormulaPartTypeFromString(t.GetString() ?? string.Empty) != CalculationPartType.UnknownPart)
            {
                Parts = [GameCalculationPart.PartFromJson(subPart, context)];
                return;
            }
            Parts = subPart.EnumerateObject().Select(e => GameCalculationPart.PartFromJson(e.Value, context)).ToList();
            return;
        }

        if (!element.TryGetProperty("mSubparts", out JsonElement subParts)) return;
        Parts = subParts.EnumerateArray().Select(e => GameCalculationPart.PartFromJson(e, context)).ToList();
    }

    public CalculationPartType Type() => CalculationPartType.SumOfSubPartsCalculationPart;
}