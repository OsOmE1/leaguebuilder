namespace LeagueBuilder.Data.Models;

public class CalculationContext
{
    public ChampionInstance Champion;
    public Dictionary<string, DataValue> DataValues;
    public List<List<double>> EffectValues;
    public int SpellRank;
    public double Multiplier;
    public StringResolver StringResolver;
}