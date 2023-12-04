using System.Text.Json;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class ItemsByRarityWithCoefficient : IGameCalculationPart
{
    private double Coefficient;
    private int Epicness;

    public double GetValue(CalculationContext context)
    {
        return Coefficient * context.Champion.Items.Count(i => i.Epicness == Epicness);
    }

    public string String(CalculationContext context) => $"{Math.Round(Coefficient * 100)}% (items with e={Epicness})";

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("Coefficient", out JsonElement coef))
            Coefficient = coef.GetDouble();
        if (element.TryGetProperty("epicness", out JsonElement epic))
            Epicness = epic.GetInt32();
    }

    public CalculationPartType Type() => CalculationPartType.ItemsByRarityWithCoefficient;
}