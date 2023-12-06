using System.Text;
using System.Text.Json;
using LeagueBuilder.Calc.Parts;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc;

public class GameCalculation : IGameCalculation
{
    private string _name;
    public List<IGameCalculationPart> FormulaParts;
    public bool DisplayAsPercent;
    public int Precision;
    public IGameCalculationPart? Multiplier;

    public string Name() => _name;

    public double Value(CalculationContext context)
    {
        if (Multiplier != null) context.Multiplier *= Multiplier.GetValue(context);
        return FormulaParts.Sum(part => part.GetValue(context) * context.Multiplier);
    }

    public string String(CalculationContext context)
    {
        var parts = new List<string>();
        foreach (string val in FormulaParts.Select(part => part.String(context)))
        {
            if (!double.TryParse(val, out double v))
            {
                parts.Add(val);
                continue;
            }
            if (!DisplayAsPercent)
            {
                parts.Add($"{Math.Round(v)}");
                continue;
            }

            if (v > 1) return $"{Math.Round(v)}%";
            v *= 100;
            parts.Add($"{Math.Round(v)}%");
        }

        string sum = string.Join(" + ", parts);
        return Multiplier == null ? sum : $"({sum}) * {Multiplier.String(context)}";
    }

    public void FromJson(string name, JsonElement element, ParseContext context)
    {
        _name = name;
        FormulaParts = element.GetProperty("mFormulaParts")
            .EnumerateArray()
            .Select(p => GameCalculationPart.PartFromJson(p, context))
            .Where(p => p != null)
            .ToList()!;
        if (element.TryGetProperty("mMultiplier", out JsonElement multPart))
            Multiplier = GameCalculationPart.PartFromJson(multPart, context);
        if (element.TryGetProperty("mDisplayAsPercent", out JsonElement disp))
            DisplayAsPercent = disp.GetBoolean();
        if (element.TryGetProperty("mPrecision", out JsonElement percent))
            Precision = percent.GetInt32();
    }

    public IEnumerable<(StatType, StatFormulaType)> GetStatTypes() =>
        FormulaParts
            .Where(p => p.Type() is CalculationPartType.StatByCoefficientCalculationPart
                or CalculationPartType.StatBySubPartCalculationPart
                or CalculationPartType.SubPartScaledProportionalToStat
                or CalculationPartType.StatByNamedDataValueCalculationPart)
            .Select(p => ((IGameCalculationPartWithStats)p).GetStat());


    public CalculationType Type() => CalculationType.GameCalculationType;
}