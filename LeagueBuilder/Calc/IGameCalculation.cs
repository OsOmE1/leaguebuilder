using System.Text.Json;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc;

public interface IGameCalculation
{
    public string Name();
    public double Value(CalculationContext context);
    public string String(CalculationContext context);
    public void FromJson(string name, JsonElement element, ParseContext context);
    public CalculationType Type();
}