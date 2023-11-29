using System.Text.Json;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class StatByNamedDataValueCalculationPart  : IGameCalculationPart
{
    private DataValue? DataValue;
    private StatType Stat;
    private StatFormulaType StatFormula;

    public double GetValue(CalculationContext context) =>
        (DataValue?.Values[context.SpellRank] > 3
            ? DataValue?.Values[context.SpellRank] / 100
            : DataValue?.Values[context.SpellRank]) * context.Champion.GetStat(Stat, StatFormula) ?? -1;

    public string String(CalculationContext context) =>
        $"{Math.Round((DataValue?.Values[context.SpellRank] > 3
            ? DataValue?.Values[context.SpellRank]
            : DataValue?.Values[context.SpellRank] * 100) ?? -1)}% {StatFormula}{Stat}";

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mStat", out JsonElement stat))
            Stat = (StatType)stat.GetInt32();
        if (element.TryGetProperty("mStatFormula", out JsonElement form))
            StatFormula = (StatFormulaType)form.GetInt32();
        if (!element.TryGetProperty("mDataValue", out JsonElement dv)) return;
        DataValue = context.FindDataValue(dv.GetString()!);
        if (DataValue == null) throw new Exception("bruh");
    }

    public CalculationPartType Type() => CalculationPartType.StatByNamedDataValueCalculationPart;
}