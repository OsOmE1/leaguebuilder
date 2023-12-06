using LeagueBuilder;
using LeagueBuilder.Data;

Config config = Config.FromFile("config.yml");

var data = new DataDragon(config);

var champion = data.GetChampion("Shyvana");
var instance = champion.GetInstance(1);

foreach (Spell spell in champion.Spells)
{
    Console.WriteLine($"{spell.Title}: (deals: {string.Join(",", spell.GetDamageTypes())}) " +
                      $"(scales with: {string.Join(",", spell.GetScalingStats().Select(t => Utils.StatToStringShort(t.Item1, t.Item2)))})");
    Console.WriteLine(spell.ResolveSpellText(instance, 1, SpellResolveMode.Symbolic));
    Console.WriteLine();
}
