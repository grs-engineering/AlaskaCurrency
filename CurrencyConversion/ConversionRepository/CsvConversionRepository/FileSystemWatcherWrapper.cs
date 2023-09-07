namespace CurrencyConversion.ConversionRepository.CsvConversionRepository;

public class FileSystemWatcherWrapper : IDirectoryChangedWatcher
{
    private readonly FileSystemWatcher _watcher;
    
    public event FileSystemEventHandler Changed;

    public FileSystemWatcherWrapper(FileSystemWatcher watcher)
    {
        _watcher = watcher;
        watcher.Changed += Changed;
    }

    public bool EnableRaisingEvents
    {
        get => _watcher.EnableRaisingEvents;
        set => _watcher.EnableRaisingEvents = value;
    }
}