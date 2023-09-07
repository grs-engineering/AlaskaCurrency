namespace CurrencyConversion.ConversionRepository.CsvConversionRepository;

public interface ICurrencyDirectoryWatcher
{
    /// <summary>
    /// Time at which the repo was last reloaded
    /// </summary>
    DateTime LastUpdated { get; }

    /// <summary>
    /// Current repo instance
    /// </summary>
    ICurrencyConversionRepository? CurrentRepository { get; }

    /// <summary>
    /// Loads the repo from the current directory
    /// </summary>
    /// <returns></returns>
    Task LoadCurrent();
}