using CurrencyConversion.Money;

namespace CurrencyConversion.ConversionRepository;

public class InMemoryConversionRepository : ICurrencyConversionRepository
{
    private readonly IDictionary<Currency, MoneyRate> _conversionRates;
    
    public InMemoryConversionRepository(IDictionary<Currency, MoneyRate> conversionRates) =>
        _conversionRates = conversionRates;

    public Task<MoneyRate?> GetUsConversionByCountryCodeAsync(string countryCode) =>
        Task.FromResult(
            _conversionRates.TryGetValue(
                new Currency(countryCode, countryCode),
                out var value) ?
            value :
            (MoneyRate?) null);
}