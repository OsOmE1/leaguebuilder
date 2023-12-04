using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class ClampBySubpartCalculationPart : IGameCalculationPart
{
    private double _floor;
    private double _ceiling;
    private List<IGameCalculationPart> Parts;

    public double GetValue(CalculationContext context) => Math.Clamp(Parts.Aggregate(1.0, (a, p) => a * p.GetValue(context)), _floor, _ceiling);

    public string String(CalculationContext context) => $"clamp({string.Join(" * ", Parts.Select(p => p.String(context)))}, {_floor}, {_ceiling})";

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mCeiling", out JsonElement ceil))
            _ceiling = ceil.GetDouble();
        if (element.TryGetProperty("mFloor", out JsonElement floor))
            _floor = floor.GetDouble();
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

    public CalculationPartType Type() => CalculationPartType.ClampBySubpartCalculationPart;
}