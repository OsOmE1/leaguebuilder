using LeagueBuilder;
using LeagueBuilder.Data;

Config config = Config.FromFile("config.yml");
config.KeyReplacements.Add("{1d5e6fafb2}", "Spell_HweiW_Name");
config.KeyReplacements.Add("{4af8444a14}", "Spell_HweiE_Name");
config.KeyReplacements.Add("{2cadf90cce}", "Spell_HweiPassive_Name");
config.KeyReplacements.Add("{3b16368448}", "Spell_HweiE_Tooltip");
config.KeyReplacements.Add("{0f9cca49f9}", "Spell_HweiQ_Tooltip");
config.KeyReplacements.Add("{76eeb3f4c0}", "Spell_HweiR_Tooltip");
config.KeyReplacements.Add("{06a2953729}", "Spell_HweiPassive_Tooltip");
config.KeyReplacements.Add("{09d89d693b}", "Spell_HweiQQ_Tooltip");
config.KeyReplacements.Add("{1f97c1e3fa}", "Spell_HweiQW_Name");
config.KeyReplacements.Add("{0c1556cff6}", "Spell_HweiQW_Tooltip");
config.KeyReplacements.Add("{3a702c9c52}", "Spell_HweiQE_Name");
config.KeyReplacements.Add("{2460fde281}", "Spell_HweiQE_Tooltip");
config.KeyReplacements.Add("{4060f7625c}", "Spell_HweiWW_Name");
config.KeyReplacements.Add("{33e1afda44}", "Spell_HweiEQ_Name");
config.KeyReplacements.Add("{5548459b18}", "Spell_HweiEE_Name");


var data = new DataDragon(config);

var champion = data.GetChampion("Hwei");
var instance = champion.GetInstance(1);

foreach (Spell spell in champion.Spells)
{
    Console.WriteLine($"{spell.Title}:");
    Console.WriteLine(spell.ResolveSpellText(instance, 1, SpellResolveMode.Symbolic));
    Console.WriteLine();
}
