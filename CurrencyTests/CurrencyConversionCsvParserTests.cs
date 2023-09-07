using CurrencyConversion;
using CurrencyConversion.ConversionRepository.CsvConversionRepository;
using CurrencyConversion.Money;

namespace CurrencyTests;

public class CurrencyConversionCsvParserTests
{
    private CurrencyConversionCsvParser Parser { get; } = new();

    [Fact]
    public async Task Parser_Accepts_EmptyFile()
    {
        using var reader = BuildReader();
        var repo = await Parser.LoadIntoRepositoryAsync(reader);
        
        Assert.NotNull(repo);
    }

    [Fact]
    public async Task Parser_Fails_MalformedFile()
    {
        using var reader = BuildReader(
            "A,B,C",
            "a,b,1");

        // TODO: wrap this exception with something specific to the app
        var e = await Assert.ThrowsAsync<CsvHelper.HeaderValidationException>(async () =>
            await Parser.LoadIntoRepositoryAsync(reader));
    }
    
    [Fact]
    public async Task ParserParses()
    {
        using var reader = BuildReader(
            CurrencyConversionCsvParser.HEADER,
            "USD,United States Dollar,1");
        
        var repo = await Parser.LoadIntoRepositoryAsync(reader);
        var findUsd = await repo.GetUsConversionByCountryCodeAsync("USD");

        Assert.Equal(new MoneyRate(Currency.USD, Currency.USD, 1), findUsd);
    }

    /// <summary>
    /// This test is to warn you that subsequent values for the same country code will overwrite any others that exist.
    /// </summary>
    [Fact]
    public async Task GOTCHA_Parser_OverwritesValues()
    {
        using var reader = BuildReader(
            CurrencyConversionCsvParser.HEADER,
            "USD,United States Dollar,2.71828",
            "USD,United States Dollar (Corrected),1");
        
        var repo = await Parser.LoadIntoRepositoryAsync(reader);
        var findUsd = await repo.GetUsConversionByCountryCodeAsync("USD");

        Assert.Equal(new MoneyRate(
            Currency.USD, Currency.USD, 1)
            , findUsd);
    }

    private TextReader BuildReader(params string[] lines)
    {
        var text = string.Join('\n', lines.Select(l => l.TrimEnd('\n')));
        return new StringReader(text);
    }
}