using System.Text.Json;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc;

public class GameCalculationConditional : IGameCalculation
{
    private string _name;
    public GameCalculation DefaultGameCalculation;
    public GameCalculation ConditionalGameCalculation;

    public int OverrideSpellLevel;
    public IGameCalculationPart? Multiplier;

    public string Name()
    {
        return _name;
    }

    public double Value(CalculationContext context)
    {
        return DefaultGameCalculation.Value(context);
    }

    public string String(CalculationContext context)
    {
        return DefaultGameCalculation.String(context);
    }

    public void FromJson(string name, JsonElement element, ParseContext context)
    {
        _name = name;

        if (element.TryGetProperty("mDefaultGameCalculation", out JsonElement defaultCalName))
        {
            if (context.RawSpell.MSpell.MSpellCalculations != null
                && context.RawSpell.MSpell.MSpellCalculations.RootElement.TryGetProperty(defaultCalName.GetString() ?? string.Empty, out JsonElement cal))
            {
                var gc = new GameCalculation();
                gc.FromJson(name, cal, context);
                DefaultGameCalculation = gc;
            }
        }

        // get property {c0482365} for conditional
        if (!element.TryGetProperty("mConditionalGameCalculation", out JsonElement conditionalCalName)) return;
        if (context.RawSpell.MSpell.MSpellCalculations == null
            || !context.RawSpell.MSpell.MSpellCalculations.RootElement.TryGetProperty(
                conditionalCalName.GetString() ?? string.Empty, out JsonElement cal1)) return;
        {
            var gc = new GameCalculation();
            gc.FromJson(name, cal1, context);
            ConditionalGameCalculation = gc;
        }
    }

    public IEnumerable<(StatType, StatFormulaType)> GetStatTypes() => DefaultGameCalculation.GetStatTypes();

    public CalculationType Type() => CalculationType.GameCalculationConditional;
}