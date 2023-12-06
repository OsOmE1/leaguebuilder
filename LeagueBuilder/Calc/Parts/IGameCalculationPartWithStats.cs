using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Calc.Parts;

public interface IGameCalculationPartWithStats
{
    (StatType, StatFormulaType) GetStat();
}