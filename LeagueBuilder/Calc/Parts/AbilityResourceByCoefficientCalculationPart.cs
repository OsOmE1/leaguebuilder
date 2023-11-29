using System.Text.Json;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public class AbilityResourceByCoefficientCalculationPart : IGameCalculationPart
{
    private double Coefficient;
    private AbilityResourceType AbilityResource;
    private StatFormulaType StatFormula;

    public double GetValue(CalculationContext context)
    {
        throw new NotImplementedException("ability resource values not implemented");
        // ctx.Champion.GetAbilityResourceValue(p.AbilityResource, p.StatFormula)
    }

    public string String(CalculationContext context) => $"{Math.Round(Coefficient * 100)}% {AbilityResource}";

    public void FromJson(JsonElement element, ParseContext context)
    {
        if (element.TryGetProperty("mCoefficient", out JsonElement coef))
            Coefficient = coef.GetDouble();
        if (element.TryGetProperty("mStatFormula", out JsonElement stat))
            StatFormula = (StatFormulaType)stat.GetInt32();
        if (element.TryGetProperty("mAbilityResource", out JsonElement resource))
            AbilityResource = (AbilityResourceType)resource.GetInt32();
    }

    public CalculationPartType Type() => CalculationPartType.AbilityResourceByCoefficientCalculationPart;
}