using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class SubPartScaledProportionalToStat : IGameCalculationPart
{
    private IGameCalculationPart Part;
    private StatType Stat;
    private StatFormulaType StatFormula;
    private double Ratio;

    public double GetValue(CalculationContext context) => Part.GetValue(context) * (context.Champion.GetStat(Stat, StatFormula) * Ratio + 1);

    public string String(CalculationContext context) => $"(1 + {Math.Round(Part.GetValue(context) * 100)} {StatFormula}{Stat}%)";

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mStat", out JsonElement stat))
            Stat = (StatType)stat.GetInt32();
        if (element.TryGetProperty("mStatFormula", out JsonElement form))
            StatFormula = (StatFormulaType)form.GetInt32();
        if (element.TryGetProperty("mSubpart", out JsonElement part))
            Part = GameCalculationPart.PartFromJson(part, context);
        if (element.TryGetProperty("mRatio", out JsonElement ratio))
            Ratio = ratio.GetDouble();
    }

    public CalculationPartType Type() => CalculationPartType.SubPartScaledProportionalToStat;
}