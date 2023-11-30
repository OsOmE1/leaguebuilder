using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class StatBySubPartCalculationPart  : IGameCalculationPart
{
    private IGameCalculationPart Part;
    private StatType Stat;
    private StatFormulaType StatFormula;

    public double GetValue(CalculationContext context) => Part.GetValue(context) * context.Champion.GetStat(Stat, StatFormula);

    public string String(CalculationContext context)
    {
        var val = Part.GetValue(context);
        if (val is > 0 and < 3) val *= 100;
        return $"{Math.Round(val)}% {StatFormula}{Stat}";
    }

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mStat", out JsonElement stat))
            Stat = (StatType)stat.GetInt32();
        if (element.TryGetProperty("mStatFormula", out JsonElement form))
            StatFormula = (StatFormulaType)form.GetInt32();
        if (element.TryGetProperty("mSubpart", out JsonElement part))
            Part = GameCalculationPart.PartFromJson(part, context)!;
    }

    public CalculationPartType Type() => CalculationPartType.StatBySubPartCalculationPart;
}