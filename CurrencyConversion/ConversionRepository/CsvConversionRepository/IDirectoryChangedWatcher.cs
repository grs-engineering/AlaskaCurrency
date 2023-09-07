namespace CurrencyConversion.ConversionRepository.CsvConversionRepository;

public interface IDirectoryChangedWatcher
{
    /// <summary>
    /// Event that will trigger when the watcher detects a change
    /// </summary>
    event FileSystemEventHandler Changed;

    /// <summary>
    /// Gets or sets a value indicating whether the watcher is enabled
    /// </summary>
    bool EnableRaisingEvents { get; set; }
}