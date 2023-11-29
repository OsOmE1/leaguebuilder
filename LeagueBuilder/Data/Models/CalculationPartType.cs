namespace LeagueBuilder.Data.Models;

public enum CalculationPartType : int
{
    UnknownPart,
    IGameCalculationPartType,
    EffectValueCalculationPart,
    NamedDataValueCalculationPart,
    CustomReductionMultiplierCalculationPart,
    ProductOfSubPartsCalculationPart,
    SumOfSubPartsCalculationPart,
    NumberCalculationPart,
    IGameCalculationPartWithStats,
    StatByCoefficientCalculationPart,
    StatBySubPartCalculationPart,
    StatByNamedDataValueCalculationPart,
    SubPartScaledProportionalToStat,
    AbilityResourceByCoefficientCalculationPart,
    IGameCalculationPartWithBuffCounter,
    BuffCounterByCoefficientCalculationPart,
    BuffCounterByNamedDataValueCalculationPart,
    IGameCalculationPartByCharLevel,
    ByCharLevelInterpolationCalculationPart,
    ByCharLevelBreakpointsCalculationPart,
    Breakpoint,
    ByCharLevelFormulaCalculationPart,
    CooldownMultiplierCalculationPart,
}