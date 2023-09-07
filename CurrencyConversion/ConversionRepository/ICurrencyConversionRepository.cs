using CurrencyConversion.Money;

namespace CurrencyConversion.ConversionRepository;

public interface ICurrencyConversionRepository
{
    /// <summary>
    /// Finds <see cref="Currency"/> by country code. Returns null if not found
    /// </summary>
    /// <param name="countryCode">Country code to search (case insensitive)</param>
    /// <returns><see cref="Currency"/> if found, otherwise null</returns>
    Task<MoneyRate?> GetUsConversionByCountryCodeAsync(string countryCode);
}