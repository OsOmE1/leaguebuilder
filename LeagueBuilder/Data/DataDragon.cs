using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using LeagueBuilder.Data.Models;

namespace LeagueBuilder.Data;

public class DataDragon
{
    private const string Base = "https://raw.communitydragon.org/";
    private readonly StringResolver _sr;
    private readonly Config _config;
    private readonly List<Item> _items;
    private readonly string _url;
    private readonly HttpClient _client;

    public DataDragon(Config config)
    {
        _config = config;
        _url = Base + config.Patch + "/";
        _client = new HttpClient();
        var res = Get<JsonDocument>($"game/data/menu/main_{config.Locale}.stringtable.json");
        if (res == null) throw new Exception("could not load fontconfig");

        var fc = res.RootElement.GetProperty("entries").Deserialize<Dictionary<string, string>>();
        if (fc == null) throw new Exception("could not cast fontconfig");
        _sr = new StringResolver(fc!);
        foreach (var keyReplacement in config.KeyReplacements)
            _sr.ReplaceKey(keyReplacement.Key,keyReplacement.Value);
        foreach (var keyLink in config.KeyLinks)
            _sr.LinkKey(keyLink.Key,keyLink.Value);
        foreach (var spell in config.SpellReplacements)
        foreach (var spellReplacement in spell.Value)
            _sr.AddSpellReplacement(spell.Key, spellReplacement.Key, spellReplacement.Value);

        _items = new List<Item>();
        res = Get<JsonDocument>($"game/items.cdtb.bin.json");
        _items = res?.RootElement.EnumerateObject()
            .Where(p => Regex.IsMatch(p.Name, @"^Items/\d+$"))
            .Select(p => p.Value.Deserialize<ApiItem>())
            .Where(i => i != null)
            .Select(i => new Item(i!, _sr))
            .ToList() ?? [];
    }

    public T? Get<T>(string path)
    {
        using var req = new HttpRequestMessage();
        req.Method = HttpMethod.Get;
        req.RequestUri = new Uri(_url + path);

        HttpResponseMessage resp = _client.Send(req);

        Stream body = resp.Content.ReadAsStream();
        return JsonSerializer.Deserialize<T>(body);
    }

    private JsonDocument? GetChampionBin(string champion)
    {
        using var req = new HttpRequestMessage();
        req.Method = HttpMethod.Get;
        req.RequestUri = new Uri(_url + $"game/data/characters/{champion.ToLower()}/{champion.ToLower()}.bin.json");

        HttpResponseMessage resp = _client.Send(req);

        TextReader reader = new StreamReader(resp.Content.ReadAsStream());
        if (!_config.BinReplacements.TryGetValue(champion, out var replacements))
        {
            Stream body = resp.Content.ReadAsStream();
            return JsonSerializer.Deserialize<JsonDocument>(body);
        }
        string text = reader.ReadToEnd();
        foreach ((string o, string n) in replacements)
            text = text.Replace(o, n);


        return JsonSerializer.Deserialize<JsonDocument>(text);
    }

    public IEnumerable<string> GetChampionNames()
    {
        var res = Get<JsonDocument>("game/global/champions/champions.bin.json");
        if (res == null) throw new Exception("could not load champions file");

        return res.RootElement
            .EnumerateObject()
            .Where(v => v.Name.Contains("Characters/"))
            .Select(v => v.Value.GetProperty("name").GetString())
            .Where(s => s != null && s != "TFTChampion")!;
    }

    public Item? GetItem(int id) => GetItem(i => i.Id == id);
    public Item? GetItem(string name) => GetItem(i => i.Title == name || i.Name == name);
    public Item? GetItem(Func<Item, bool> predicate) => _items.FirstOrDefault(predicate);
    public IEnumerable<Item> GetItems(Func<Item, bool> predicate) => _items.Where(predicate);
    public List<Item> GetItems() => _items;

    public Champion GetChampion(string name)
    {
        var res = GetChampionBin(name);
        if (res == null) throw new Exception("could not load champion file");

        ApiChampion apiChampion = new();

        var record = res.RootElement.GetProperty($"Characters/{name}/CharacterRecords/Root").Deserialize<CharacterRecord>();
        if (record == null) throw new Exception("could deserialize champion");
        apiChampion.Character = record;

        // exception
        if (name == "FiddleSticks") name = "Fiddlesticks";

        var spells = record.SpellNames.Append(record.MCharacterPassiveSpell).ToList();
        var abilities = record.MAbilities ?? [];
        for (var i = 0; i < spells.Count; i++)
        {
            var path = $"Characters/{name}/Spells/{spells[i]}";
            if (i == spells.Count - 1) path = spells[i];

            var val = Utils.GetBinValue(path, res);
            if (!val.HasValue)
            {
                Console.WriteLine($"error spell: {spells[i]}, not found in response");
                continue;
            }

            var spell = val.Value.Deserialize<ApiSpell>();
            if (spell == null)
            {
                Console.WriteLine($"error casting spell: {spells[i]} to class");
                continue;
            }
            apiChampion.Spells.Add(spell);
        }

        // find any additional spells
        foreach (JsonProperty prop in res.RootElement.EnumerateObject())
        {
            if (!Regex.IsMatch(prop.Name, $"^Characters/{name}/Spells/.+$")) continue;
            var spellName = prop.Name.Split("/").Last();

            // have already processed the spell as a main spell
            if (apiChampion.Spells.Any(s => s.MScriptName == spellName)) continue;
            var spell = prop.Value.Deserialize<ApiSpell>();
            if (spell == null)
            {
                Console.WriteLine($"error casting spell: {spellName} to class");
                continue;
            }

            // skip empty spells
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (spell.MSpell == null) continue;
            apiChampion.AltSpells.Add(spell);
        }
        foreach (string abilityName in abilities)
        {
            if (!res.RootElement.TryGetProperty(abilityName, out JsonElement val))
            {
                Console.WriteLine($"error ability: {abilityName}, not found in response");
                continue;
            }
            var ability = val.Deserialize<Ability>();
            if (ability == null)
            {
                Console.WriteLine($"error casting ability: {abilityName} to class");
                continue;
            }
            apiChampion.Abilities.Add(ability);
        }

        return new Champion(apiChampion, _sr);
    }
}