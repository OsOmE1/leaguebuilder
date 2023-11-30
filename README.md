# LeagueBuilder (WIP!)
## Description
LeagueBuilder is a C# library that can interact with the community dragon CDN to pull data on certain champions
. It can then parse this data and process calculation given various variables.

## Roadmap
* Add Runes!
* Fix small issues in tooltip resolving
* A lot of code cleanup

## How to use
```c#
// load your config
var config = Config.FromFile("config.yml");
// create new instance which we can use to retrieve data
var data = new DataDragon(config);
// get all champion names
var champs = data.GetChampionNames();
// lets take Twitch as an example
var champion = data.GetChampion("Twitch");
// champion.GetInstance(n) returns an instance of that champion at level n
var instance = champion.GetInstance(1);
// now we can for example iterate over the spells and attempt to resolve their tooltip text
foreach (Spell spell in champion.Spells)
{
    // spell.ResolveText(instance, n, mode) attempts to resolve the spell as if it was rank n
    // numerical mode reolves the variables as their absolute numerical value
    Console.WriteLine(spell.ResolveSpellText(instance, 1, SpellResolveMode.Numerical));
    // symbolic leaves symbols for stats etc. in place
    Console.WriteLine(spell.ResolveSpellText(instance, 1, SpellResolveMode.Symbolic));
}
// or get spell by name or slot
var spell = champion.GetSpell("Contaminate");
var spell = champion.GetSpell(SpellSlot.E);
// you can also get spells not designated one of the five slots using
var spell = champion.GetAltSpell("ApheliosCalibrumQ");

// you can also retrive items like this
var bork = data.GetItem(3153); // get item by id
var ie = data.GetItem("Infinity Edge"); // or name
var runaans = data.GetItem(i => 3085); // or by predicate
var dorans = data.GetItems(i => i.Title.Contains("Dorans"); // or multiple by predicate
    
// you can add the items to the champion instance as follows
instance.Items.Add(bork);
instance.Items.Add(runaans);
instance.Items.Add(ie);
// or
instance.Items.AddRange([bork, runaans, ie]);
instance.Items.AddRange(dorans);
```
Also see example in `LeagueBuilder.Example/Program.cs`

## Config
See below a simplified version of the example config with comments
```yaml
# path can be latest, pbe or any numerical version available at https://raw.communitydragon.org/
patch: latest
# in what language spell text should be resolved
locale: en_us
# key replacements replace keys in the string tables with a different value
key_replacements:
  "{15dd1b3843}": Spell_GangplankQWrapper_Tooltip_1
# key links take the value of the first key and add a duplicate entry with the second key
key_links:
  game_spell_kayn_q_main_1: game_spell_Kayn_Q_main_@f1@
# spell replacements replace specific values in spell texts (only @f<n>@ values for now)
spell_replacements:
  XayahPassive:
    f14: "@Effect6Amount@"
    f16: "@Effect8Amount*100@"
    f12: "@Effect5Amount@"

```