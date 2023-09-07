using CurrencyConversion;
using CurrencyConversion.ConversionRepository;
using CurrencyConversion.Money;

namespace CurrencyTests;

public class InMemoryMoneyRepoTests
{
    [Fact]
    public async Task CountryLookup_ReturnsNull_IfNotFound()
    {
        var repo = new InMemoryConversionRepository(new Dictionary<Currency, MoneyRate>());
        var result = await repo.GetUsConversionByCountryCodeAsync("nope");

        Assert.Null(result);
    }

    [Fact]
    public async Task CountryLookup_Finds_ByCountryCode()
    {
        var repo = GetRepo(new Money(Currency.USD, 1));
        var result = await repo.GetUsConversionByCountryCodeAsync("USD");
        
        Assert.Equal(result,
            new MoneyRate(Currency.USD, Currency.USD, 1));
    }

    private InMemoryConversionRepository GetRepo(params Money[] monies) =>
        new (monies.ToDictionary(m => m.Currency,
            m => new MoneyRate(Currency.USD, m.Currency, m.Value)));
}