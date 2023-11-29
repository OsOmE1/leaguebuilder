using System.Text.Json;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc;

public interface IGameCalculationPart
{
    public double GetValue(CalculationContext context);
    public string String(CalculationContext context);
    public void FromJson(JsonElement element, ParseContext context);
    public CalculationPartType Type();
}