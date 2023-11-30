using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class ProductOfSubPartsCalculationPart : IGameCalculationPart
{
    private IGameCalculationPart? Part1;
    private IGameCalculationPart? Part2;

    public double GetValue(CalculationContext context)
    {
        if (Part1 == null && Part2 == null) return 0;
        if (Part1 == null) return Part2!.GetValue(context);
        if (Part2 == null) return Part1.GetValue(context);
        return Part1.GetValue(context) * Part2.GetValue(context);
    }

    public string String(CalculationContext context) => $"({Part1?.String(context) ?? "1"}*{Part2?.String(context) ?? "1"})";

    public void FromJson(JsonElement element, ParseContext context)
    {
       if (element.TryGetProperty("mPart1", out JsonElement part1))
           Part1 = GameCalculationPart.PartFromJson(part1, context);
       if (element.TryGetProperty("mPart2", out JsonElement part2))
           Part2 = GameCalculationPart.PartFromJson(part2, context);
    }

    public CalculationPartType Type() => CalculationPartType.ProductOfSubPartsCalculationPart;
}