using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace LeagueBuilder.Data;

public partial class Version
{
    private const string BaseUrl = "https://raw.communitydragon.org/";

    private readonly string _url;
    private int _major;
    private int _minor;
    private int _patch;
    private string _buildMetadata;

    // https://semver.org/; semver regex
    [GeneratedRegex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$")]
    private static partial Regex SemverRegex();

    public Version(string version)
    {
        _url = BaseUrl + version + "/";

        using var tmpClient = new HttpClient();
        using var req = new HttpRequestMessage();
        req.Method = HttpMethod.Get;
        req.RequestUri = new Uri(_url + "content-metadata.json");

        HttpResponseMessage resp = tmpClient.Send(req);

        if (resp.StatusCode != HttpStatusCode.OK)
            throw new ArgumentException($"could not find data dragon with version {version}");

        Stream body = resp.Content.ReadAsStream();
        var metadata = JsonSerializer.Deserialize<JsonDocument>(body);
        if (metadata == null || !metadata.RootElement.TryGetProperty("version", out JsonElement val))
            throw new JsonException("no version property found");

        string? semVersion = val.GetString();
        if (semVersion == null)
            throw new JsonException("no version property found");

        Match match = SemverRegex().Match(semVersion);
        if (!match.Success)
            throw new ApplicationException($"invalid version property {semVersion}");

        if(!int.TryParse(match.Groups[1].Value, out int major))
            throw new ApplicationException($"invalid major version in content metadata {semVersion}");
        if(!int.TryParse(match.Groups[2].Value, out int minor))
            throw new ApplicationException($"invalid minor version in content metadata {semVersion}");
        if(!int.TryParse(match.Groups[3].Value, out int patch))
            throw new ApplicationException($"invalid patch version in content metadata {semVersion}");
        _major = major;
        _minor = minor;
        _patch = patch;
        _buildMetadata = match.Groups[5].Value;
    }

    private static readonly Version LastFontConfigVersion = new("12.22");
    private static readonly Version LastNormalStringTableVersion = new("14.3");

    public string GetStringTableUrl(string locale)
    {
        if (After(LastNormalStringTableVersion)) return $"game/{locale}/data/menu/{locale}/main.stringtable.json";
        if (After(LastFontConfigVersion)) return $"game/data/menu/main_{locale}.stringtable.json";
        return $"game/data/menu/fontconfig_{locale}.txt.json";
    }

    public string GetItemsUrl() =>
        After(LastFontConfigVersion) ? "game/items.cdtb.bin.json" : $"game/global/items/items.bin.json";

    private bool After(Version other)
    {
        if (_major > other._major) return true;
        if (_major < other._major) return false;
        if (_minor > other._minor) return true;
        return _minor < other._minor;
    }
}