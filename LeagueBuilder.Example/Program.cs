using LeagueBuilder;
using LeagueBuilder.Data;

Config config = Config.FromFile("config.yml");
var data = new DataDragon(config);
var champs = data.GetChampionNames();
foreach (string champName in champs)
{
    Champion champion = data.GetChampion(champName);
    foreach (Spell spell in champion.Spells)
    {
        string text = spell.ResolveSpellText(champion.GetInstance(1), 1);

        Console.WriteLine(text);
        Console.WriteLine();
    }
}