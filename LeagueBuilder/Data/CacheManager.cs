namespace LeagueBuilder.Data;

public class CacheManager
{
    private Config _config;
    private readonly string _path;

    public CacheManager(Config config)
    {
        _config = config;
        _path = Path.Join(Path.GetTempPath(), "leaguebuilder");

        if (!config.Cache) return;
        if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
    }

    public void CacheFile(string path, Stream fileStream)
    {
        string? fileName = path.Split("/").LastOrDefault();
        if (fileName == null) return;
        string filePath = Path.Join(_path, fileName);

        if (File.Exists(filePath)) File.Delete(filePath);
        FileStream writeStream = File.OpenWrite(filePath);

        // copy to cached file
        fileStream.CopyTo(writeStream);

        // close cache file
        writeStream.Close();
        // reset file stream head
        fileStream.Seek(0, SeekOrigin.Begin);
    }

    public bool TryGetCachedFileStream(string path, out Stream stream)
    {
        stream = Stream.Null;

        string? fileName = path.Split("/").LastOrDefault();
        if (fileName == null) return false;

        string filePath = Path.Join(_path, fileName);
        if (!File.Exists(filePath)) return false;

        stream = File.OpenRead(filePath);
        return true;
    }
}