using System.Text.Json;
using LeagueBuilder.Calc.Parts;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc;

public abstract class GameCalculationPart
{
    public static CalculationPartType FormulaPartTypeFromString(string s) {
        switch (s) {
            case "IGameCalculationPart":
                return CalculationPartType.IGameCalculationPartType;
            case "EffectValueCalculationPart":
                return CalculationPartType.EffectValueCalculationPart;
            case "NamedDataValueCalculationPart":
                return CalculationPartType.NamedDataValueCalculationPart;
            case "CustomReductionMultiplierCalculationPart":
                return CalculationPartType.CustomReductionMultiplierCalculationPart;
            case "ProductOfSubPartsCalculationPart":
                return CalculationPartType.ProductOfSubPartsCalculationPart;
            case "SumOfSubPartsCalculationPart":
                return CalculationPartType.SumOfSubPartsCalculationPart;
            case "NumberCalculationPart":
                return CalculationPartType.NumberCalculationPart;
            case "IGameCalculationPartWithStats":
                return CalculationPartType.IGameCalculationPartWithStats;
            case "StatByCoefficientCalculationPart":
                return CalculationPartType.StatByCoefficientCalculationPart;
            case "StatBySubPartCalculationPart":
                return CalculationPartType.StatBySubPartCalculationPart;
            case "StatByNamedDataValueCalculationPart":
                return CalculationPartType.StatByNamedDataValueCalculationPart;
            case "SubPartScaledProportionalToStat":
                return CalculationPartType.SubPartScaledProportionalToStat;
            case "AbilityResourceByCoefficientCalculationPart":
                return CalculationPartType.AbilityResourceByCoefficientCalculationPart;
            case "IGameCalculationPartWithBuffCounter":
                return CalculationPartType.IGameCalculationPartWithBuffCounter;
            case "BuffCounterByCoefficientCalculationPart":
                return CalculationPartType.BuffCounterByCoefficientCalculationPart;
            case "BuffCounterByNamedDataValueCalculationPart":
                return CalculationPartType.BuffCounterByNamedDataValueCalculationPart;
            case "IGameCalculationPartByCharLevel":;
                return CalculationPartType.IGameCalculationPartByCharLevel;
            case "ByCharLevelInterpolationCalculationPart":
                return CalculationPartType.ByCharLevelInterpolationCalculationPart;
            case "ByCharLevelBreakpointsCalculationPart":
                return CalculationPartType.ByCharLevelBreakpointsCalculationPart;
            case "Breakpoint":
                return CalculationPartType.Breakpoint;
            case "ByCharLevelFormulaCalculationPart":
                return CalculationPartType.ByCharLevelFormulaCalculationPart;
            default:
                return CalculationPartType.UnknownPart;
        }
    }
    public static IGameCalculationPart? PartFromJson(JsonElement element, ParseContext context)
    {
        if (!element.TryGetProperty("__type", out JsonElement t)) return null;
        CalculationPartType partType = FormulaPartTypeFromString(t.GetString() ?? "");
        IGameCalculationPart? p = null;
        switch (partType) {
            case CalculationPartType.UnknownPart:
                return p;
            case CalculationPartType.EffectValueCalculationPart:
                p = new EffectValueCalculationPart();
                break;
            case CalculationPartType.NamedDataValueCalculationPart:
                p = new NamedDataValueCalculationPart();
                break;
            case CalculationPartType.CustomReductionMultiplierCalculationPart:
                p = new CustomReductionMultiplierCalculationPart();
                break;
            case CalculationPartType.ProductOfSubPartsCalculationPart:
                p = new ProductOfSubPartsCalculationPart();
                break;
            case CalculationPartType.SumOfSubPartsCalculationPart:
                p = new SumOfSubPartsCalculationPart();
                break;
            case CalculationPartType.NumberCalculationPart:
                p = new NumberCalculationPart();
                break;
            case CalculationPartType.StatByCoefficientCalculationPart:
                p = new StatByCoefficientCalculationPart();
                break;
            case CalculationPartType.StatBySubPartCalculationPart:
                p = new StatBySubPartCalculationPart();
                break;
            case CalculationPartType.StatByNamedDataValueCalculationPart:
                p = new StatByNamedDataValueCalculationPart();
                break;
            case CalculationPartType.SubPartScaledProportionalToStat:
                p = new SubPartScaledProportionalToStat();
                break;
            case CalculationPartType.AbilityResourceByCoefficientCalculationPart:
                p = new AbilityResourceByCoefficientCalculationPart();
                break;
            case CalculationPartType.BuffCounterByCoefficientCalculationPart:
                p = new BuffCounterByCoefficientCalculationPart();
                break;
            case CalculationPartType.BuffCounterByNamedDataValueCalculationPart:
                p = new BuffCounterByNamedDataValueCalculationPart();
                break;
            case CalculationPartType.ByCharLevelInterpolationCalculationPart:
                p = new ByCharLevelInterpolationCalculationPart();
                break;
            case CalculationPartType.ByCharLevelBreakpointsCalculationPart:
                p = new ByCharLevelBreakpointsCalculationPart();
                break;
            case CalculationPartType.ByCharLevelFormulaCalculationPart:
                p = new ByCharLevelFormulaCalculationPart();
                break;
            case CalculationPartType.CooldownMultiplierCalculationPart:
                p = new CooldownMultiplierCalculationPart();
                break;
        }

        p?.FromJson(element, context);
        return p;
    }
}