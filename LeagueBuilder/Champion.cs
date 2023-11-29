using LeagueBuilder.Data;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder;

public class Champion
{
    public StringResolver Resolver;
    public string Name;
    public List<string> Tags;

    public AbilityResourceType AbilityResource;
    public Stats BaseStats;
    public Dictionary<StatType, double> PerLevel;
    public List<Spell> Spells;
    public List<Spell> AltSpells;

    public Champion(ApiChampion champion, StringResolver stringResolver)
    {
        Resolver = stringResolver;
        Name = champion.Character.Name;
        AbilityResource = (AbilityResourceType)champion.Character.PrimaryAbilityResource.ArType;
        BaseStats = new Stats(champion.Character);
        PerLevel = new Dictionary<StatType, double>
        {
            { StatType.Attack, champion.Character.DamagePerLevel },
            { StatType.AttackSpeed, champion.Character.AttackSpeedPerLevel },
            { StatType.MaxHealth, champion.Character.HpPerLevel },
            { StatType.HealthRegenRate, champion.Character.HpRegenPerLevel },
            { StatType.Armor, champion.Character.ArmorPerLevel },
            { StatType.MagicResist, champion.Character.SpellBlockPerLevel },
            { StatType.ResourceRegenRate, champion.Character.PrimaryAbilityResource.ArRegenPerLevel }
        };
        Spells = champion.Spells.Select(s => new Spell(s, stringResolver)).ToList();
        AltSpells = champion.AltSpells.Select(s => new Spell(s, stringResolver)).ToList();
    }

    public ChampionInstance GetInstance(int level) => new(this, level);
}