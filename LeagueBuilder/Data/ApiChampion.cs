using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Data;

public class ApiChampion
{
	public CharacterRecord Character;
	public List<ApiSpell> Spells;
	public List<ApiSpell> AltSpells;
	public List<Ability> Abilities;

	public ApiChampion()
	{
		Spells = new List<ApiSpell>();
		AltSpells = new List<ApiSpell>();
		Abilities = new List<Ability>();
	}
}