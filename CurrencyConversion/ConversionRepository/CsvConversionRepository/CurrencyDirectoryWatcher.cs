namespace CurrencyConversion.ConversionRepository.CsvConversionRepository;

public class CurrencyDirectoryWatcher : ICurrencyDirectoryWatcher
{
    private readonly IDirectoryChangedWatcher _fileSystemWatcher;
    private readonly ICurrencyConversionCsvParser _parser;
    private readonly string _directory;
    
    private const string Pattern = "Conversion*.csv";

    public CurrencyDirectoryWatcher(
        IDirectoryChangedWatcher watcher,
        ICurrencyConversionCsvParser parser,
        string directory)
    {
        _fileSystemWatcher = watcher;
        _fileSystemWatcher.Changed += async (sender, args) => await Update(args.FullPath);
        
        _parser = parser;
        _directory = directory;
    }

    /// <inheritdoc/>
    public DateTime LastUpdated { get; private set; } = DateTime.Now;

    /// <inheritdoc/>
    public ICurrencyConversionRepository? CurrentRepository { get; private set; }

    public async Task LoadCurrent()
    {
        var dir = new DirectoryInfo(_directory);
        var file = dir.EnumerateFiles(Pattern).MaxBy(f => f.LastWriteTime);
        await Update(file!.FullName);
    }
    
    private async Task Update(string filename)
    {
        CurrentRepository = await _parser.LoadIntoRepositoryAsync(filename);
        LastUpdated = DateTime.Now;
    }
}