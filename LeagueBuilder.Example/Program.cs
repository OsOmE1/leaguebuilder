using LeagueBuilder;
using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

Config config = Config.FromFile("config.yml");

var data = new DataDragon(config);

Champion champion = data.GetChampion("Twitch");
ChampionInstance instance = champion.GetInstance(6);

Item bork = data.GetItem(3153)!;
Item ie = data.GetItem("Infinity Edge")!;
Item runaans = data.GetItem(3085)!;
instance.Items.AddRange([bork, runaans, ie]);

instance.SetStatMod(StatModSlot.Offense, ShardType.AttackSpeed);
instance.SetStatMod(StatModSlot.Flex, ShardType.Adaptive);
instance.SetStatMod(StatModSlot.Defense, ShardType.Armor);

Spell spell = champion.GetSpell(SpellSlot.E)!;
Console.WriteLine(spell.ResolveSpellText(instance, 5, SpellResolveMode.Numerical));
// foreach (Spell spell in champion.Spells)
// {
//     Console.WriteLine($"{spell.Title}: (deals: {string.Join(",", spell.GetDamageTypes())}) " +
//                       $"(scales with: {string.Join(",", spell.GetScalingStats().Select(t => Utils.StatToStringShort(t.Item1, t.Item2)))})");
//     Console.WriteLine(spell.ResolveSpellText(instance, 1, SpellResolveMode.Symbolic));
//     Console.WriteLine();
// }
