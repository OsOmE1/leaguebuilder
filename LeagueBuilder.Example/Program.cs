using LeagueBuilder;
using LeagueBuilder.Data;

Config config = Config.FromFile("config.yml");

var data = new DataDragon(config);

var champion = data.GetChampion("Hwei");
var instance = champion.GetInstance(1);

foreach (Spell spell in champion.Spells)
{
    Console.WriteLine($"{spell.Title}:");
    Console.WriteLine(spell.ResolveSpellText(instance, 1, SpellResolveMode.Symbolic));
    Console.WriteLine();
}
