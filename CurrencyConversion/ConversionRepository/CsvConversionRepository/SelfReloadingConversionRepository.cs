namespace CurrencyConversion.ConversionRepository.CsvConversionRepository;

public class SelfReloadingConversionRepository : ICurrencyConversionRepository
{
    private readonly ICurrencyDirectoryWatcher _directoryWatcher;

    public SelfReloadingConversionRepository(ICurrencyDirectoryWatcher directoryWatcher) =>
        _directoryWatcher = directoryWatcher;
    
    public async Task<Money.MoneyRate?> GetUsConversionByCountryCodeAsync(string countryCode)
    {
        // This will maintain a consistent reference this repository for this transaction,
        //  so if the watcher updates there is no conflict
        var repo = _directoryWatcher.CurrentRepository;

        if (repo == null)
        {
            await _directoryWatcher.LoadCurrent();
            repo = _directoryWatcher.CurrentRepository;

            if (repo == null)
            {
                throw new Exception("Could not load currency file");
            }
        }
        return await repo.GetUsConversionByCountryCodeAsync(countryCode);
    }
}