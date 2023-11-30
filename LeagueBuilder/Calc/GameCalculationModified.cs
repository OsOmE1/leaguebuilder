using System.Text.Json;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc;

public class GameCalculationModified : IGameCalculation
{
    private string _name;
    public GameCalculation ModifiedGameCalculation;
    public int OverrideSpellLevel;
    public IGameCalculationPart? Multiplier;

    public string Name()
    {
        return _name;
    }

    public double Value(CalculationContext context)
    {
        if (Multiplier == null) return 0;
        return ModifiedGameCalculation.Value(context) * Multiplier.GetValue(context);
    }

    public string String(CalculationContext context)
    {
        if (Multiplier == null) return "?";
        return ModifiedGameCalculation.String(context) + " * " + $"{Multiplier.GetValue(context)}";
    }

    public void FromJson(string name, JsonElement element, ParseContext context)
    {
        _name = name;

        name = element.GetProperty("mModifiedGameCalculation").GetString() ?? string.Empty;
        if (element.TryGetProperty("mMultiplier", out JsonElement multPart))
            Multiplier = GameCalculationPart.PartFromJson(multPart, context);
        if (context.RawSpell.MSpell.MSpellCalculations.RootElement.TryGetProperty(name, out JsonElement cal))
        {
            var gc = new GameCalculation();
            gc.FromJson(name, cal, context);
            ModifiedGameCalculation = gc;
        }

        OverrideSpellLevel = -1;
        if (element.TryGetProperty("mOverrideSpellLevel", out JsonElement level))
            OverrideSpellLevel = level.GetInt32();
    }

    public CalculationType Type() => CalculationType.GameCalculationModifiedType;
}