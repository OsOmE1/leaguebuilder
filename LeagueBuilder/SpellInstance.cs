using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder;

public class SpellInstance
{
    public Spell Spell;
    public int Rank;

    public SpellInstance(Spell spell, int rank)
    {
        Spell = spell;
        Rank = rank;
    }
}