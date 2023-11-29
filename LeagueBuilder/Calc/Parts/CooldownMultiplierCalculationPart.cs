using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class CooldownMultiplierCalculationPart  : IGameCalculationPart
{
    public double GetValue(CalculationContext context) => 0; // TODO: implement

    public string String(CalculationContext context) => "";

    public void FromJson(JsonElement element, ParseContext context) { }

    public CalculationPartType Type() => CalculationPartType.CooldownMultiplierCalculationPart;
}