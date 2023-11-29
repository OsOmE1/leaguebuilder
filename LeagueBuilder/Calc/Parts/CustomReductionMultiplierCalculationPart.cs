using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class CustomReductionMultiplierCalculationPart : IGameCalculationPart
{
    private double MaximumReductionPercent;
    private IGameCalculationPart? Part;

    public double GetValue(CalculationContext context)
    {
        if (Part == null) return 0;
        return 1 - Math.Clamp(Part.GetValue(context), 0, MaximumReductionPercent);
    }

    public string String(CalculationContext context) => $"{Math.Round(GetValue(context)*100)}%";

    public void FromJson(JsonElement element, ParseContext context)
    {
       JsonProperty? v = element.EnumerateObject().FirstOrDefault(v => v.Name.Contains('{'));
       if (v != null) Part = GameCalculationPart.PartFromJson(element.GetProperty(v.Value.Value.GetString()!), context);
       if (element.TryGetProperty("mMaximumReductionPercent", out JsonElement start))
           MaximumReductionPercent = start.GetDouble();
    }

    public CalculationPartType Type() => CalculationPartType.CustomReductionMultiplierCalculationPart;
}