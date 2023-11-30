using LeagueBuilder;
using LeagueBuilder.Data;

Config config = Config.FromFile("config.yml");
var data = new DataDragon(config);

var bork = data.GetItem(3153);
var runaans = data.GetItem(3085);
var ie = data.GetItem(3031);

var champion = data.GetChampion("Twitch");
var instance = champion.GetInstance(6);

instance.Items.AddRange([bork, runaans, ie]);
var spell = champion.GetSpell(SpellSlot.E);

Console.WriteLine(spell.ResolveSpellText(instance, 1, SpellResolveMode.Symbolic));
