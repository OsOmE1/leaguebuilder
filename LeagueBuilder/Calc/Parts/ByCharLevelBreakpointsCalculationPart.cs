using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class ByCharLevelBreakpointsCalculationPart  : IGameCalculationPart
{
    private double BaseValue;
    private double Scale;
    private Dictionary<int, double>? BreakPoints;

    public double GetValue(CalculationContext context)
    {
        double val = BaseValue;
        double perLevel = Scale;
        for (var i = 1; i < context.Champion.Level; i++)
        {
            if (BreakPoints?.TryGetValue(i + 1, out double n) ?? false) perLevel = n;
            val += perLevel;
        }

        return val;
    }

    public string String(CalculationContext context)
    {
        double val = GetValue(context);
        return val > 3 ? $"{Math.Round(val)}" : $"{val}";
    }

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mLevel1Value", out JsonElement baseValue))
            BaseValue = baseValue.GetDouble();
        foreach (JsonProperty el in element.EnumerateObject()
                     .Where(el => el.Name is not ("mLevel1Value" or "mBreakpoints" or "__type")))
            if (el.Value.TryGetDouble(out double scale)) Scale = scale;
        if (!element.TryGetProperty("mBreakpoints", out JsonElement breakPoints))
            return;
        BreakPoints = new Dictionary<int, double>();
        foreach (JsonElement b in breakPoints.EnumerateArray())
        {
            JsonProperty v = b.EnumerateObject().FirstOrDefault(e => e.Name is not ("__type" or "mLevel"));
            if (!b.TryGetProperty("mLevel", out JsonElement lvl))
            {
                if (v.Value.ValueKind == JsonValueKind.Undefined) break;
                Scale = b.GetProperty(v.Name).GetDouble();
                break;
            }
            if (v.Value.ValueKind == JsonValueKind.Undefined) break;
            BreakPoints[lvl.GetInt32()] = b.GetProperty(v.Name).GetDouble();
        }

        if (BreakPoints == null)
        {
            return;
        }
    }

    public CalculationPartType Type() => CalculationPartType.ByCharLevelBreakpointsCalculationPart;
}