using System.Text.Json;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class StatByCoefficientCalculationPart : IGameCalculationPart
{
    private double Coefficient;
    private StatType Stat;
    private StatFormulaType StatFormula;

    public double GetValue(CalculationContext context)
    {
        double val = Coefficient;
        if (val is > 0 and < 0.01) val *= 100;
        return val * context.Champion.GetStat(Stat, StatFormula);
    }

    public string String(CalculationContext context)
    {
        double val = Coefficient;
        if (val is > 0 and < 0.01) val *= 100;
        if (val is > 0 and < 3) val *= 100;
        return $"{Math.Round(val)}% {StatFormula}{Stat}";
    }

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mCoefficient", out JsonElement coef))
            Coefficient = coef.GetDouble();
        if (element.TryGetProperty("mStat", out JsonElement stat))
            Stat = (StatType)stat.GetInt32();
        if (element.TryGetProperty("mStatFormula", out JsonElement form))
            StatFormula = (StatFormulaType)form.GetInt32();
    }

    public CalculationPartType Type() => CalculationPartType.StatByCoefficientCalculationPart;
}