using System.Globalization;
using CsvHelper;
using CurrencyConversion.Money;

namespace CurrencyConversion.ConversionRepository.CsvConversionRepository;

public class CurrencyConversionCsvParser : ICurrencyConversionCsvParser
{
    public const string HEADER = "CountryCode,CurrencyName,RateFromUSDToCurrency";

    public async Task<ICurrencyConversionRepository> LoadIntoRepositoryAsync(string filename)
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException("Cannot find CSV file.", filename);
        }

        using var reader = new StreamReader(filename);
        
        return await LoadIntoRepositoryAsync(reader);
    }

    public async Task<ICurrencyConversionRepository> LoadIntoRepositoryAsync(TextReader reader)
    {
        // NOTE: Using current culture for worldwide deployments.
        // File format must be in sync with deployed culture.
        using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);

        var csvType = new
        {
            CountryCode = string.Empty,
            CurrencyName = string.Empty,
            RateFromUSDToCurrency = decimal.Zero,
        };

        var conversionToUsd = new Dictionary<Currency, MoneyRate>();
        await foreach (var record in csv.GetRecordsAsync(csvType))
        {
            if (record == null)
            {
                continue;
            }

            var targetCurrency = new Currency(record.CountryCode, record.CurrencyName);
            
            // RateFromUSDToCurrency is misnamed, the listed value is the given currency TO USD
            var rate = new MoneyRate(targetCurrency, Currency.USD, record.RateFromUSDToCurrency);

            // NOTE: This will stomp on any existing values without warning
            conversionToUsd[targetCurrency] = rate;
        }

        return new InMemoryConversionRepository(conversionToUsd);
    }
}