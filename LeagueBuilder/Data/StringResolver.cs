namespace LeagueBuilder.Data;

public class StringResolver
{
    private readonly Dictionary<string, string> _binMap;
    private readonly Dictionary<string, string?> _entries;
    private readonly Dictionary<string, Dictionary<string, string>> _spellReplacements;


    internal StringResolver(Dictionary<string, string?> entries)
    {
        _entries = entries;
        _binMap = new Dictionary<string, string>();
        _spellReplacements = new Dictionary<string, Dictionary<string, string>>();
    }

    public string? Get(string key)
    {
        if (key == "") return null;
        if (_entries.TryGetValue(key, out string? val)) return val;
        if (_entries.TryGetValue(key.ToLower(), out val)) return val;
        if (_entries.TryGetValue($"{{{Utils.StringTableHash(key)}}}", out val)) return val;

        return _binMap.TryGetValue(key, out val) ? val : null;
    }

    public string? GetSpellReplacement(string spell, string key) =>
        !_spellReplacements.TryGetValue(spell, out var value) ? null : value.GetValueOrDefault(key);

    public void AddSpellReplacement(string spell, string key, string value)
    {
        if (!_spellReplacements.ContainsKey(spell)) _spellReplacements[spell] = new Dictionary<string, string>();
        _spellReplacements[spell][key] = value;
    }

    public void AddBinMapEntry(string key, string value) => _binMap[key] = value;

    public void LinkKey(string oldKey, string newKey)
    {
        if (_entries.TryGetValue(oldKey, out string? val))
            _entries.TryAdd(newKey, val);
    }

    public void ReplaceKey(string oldKey, string newKey)
    {
        if (!_entries.Remove(oldKey, out string? val)) return;
        _entries[newKey] = val;
    }
}