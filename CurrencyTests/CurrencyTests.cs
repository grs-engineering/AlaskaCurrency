using CurrencyConversion;
using CurrencyConversion.Money;

namespace CurrencyTests;

public class CurrencyTests
{
    public static readonly object[] PositiveMatches = {
        // Just ensures we're not using ref. comparison
        new[]
        {
            new Currency("USD", "usd"),
            new Currency("USD", "usd"),
        },
        // Ignore currency name
        new[]
        {
            new Currency("USD", "usd"),
            new Currency("USD", "you ess dee"),
        },
    };

    [Theory]
    [MemberData(nameof(PositiveMatches))]
    public void Money_Compares_Positive(Currency left, Currency right)
    {
        Assert.StrictEqual(left, right);
    }

    [Fact]
    public void Money_Compares_Negative()
    {
        var left = new Currency("USD", "usd");
        var right = new Currency("FOO", "usd");

        Assert.StrictEqual(left, right);
    }
}