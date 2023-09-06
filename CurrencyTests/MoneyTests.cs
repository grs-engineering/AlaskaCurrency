using Currency;

namespace CurrencyTests;

public class MoneyTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Money_Cannot_BeLessOrEqualToZero(decimal conversionRate)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Money("a", "a", conversionRate));
        Assert.Equal(conversionRate, ex.ActualValue);
    }

    public static readonly object[] PositiveMatches = {
        // Just ensures we're not using ref. comparison
        new[]
        {
            new Money("USD", "usd", 1),
            new Money("USD", "usd", 1),
        },
        // Ignore currency name
        new[]
        {
            new Money("USD", "usd", 1),
            new Money("USD", "you ess dee", 1),
        },
    };

    [Theory]
    [MemberData(nameof(PositiveMatches))]
    public void Money_Compares_Positive(Money left, Money right)
    {
        Assert.StrictEqual(left, right);
    }

    public static readonly object[] NegativeMatches = {
        // Country code matters
        new[]
        {
            new Money("USD", "usd", 1),
            new Money("FOO","usd", 1),
        },
        // Conversion rate matters
        new[]
        {
            new Money("USD", "usd", 1),
            new Money("USD","usd", 2),
        },
    };

    [Theory]
    [MemberData(nameof(PositiveMatches))]
    public void Money_Compares_Negative(Money left, Money right)
    {
        Assert.StrictEqual(left, right);
    }

    [Fact]
    public void Money_Uppercases_CountryCode()
    {
        var money = new Money("usd", "usd", 1);
        Assert.Equal("USD", money.CountryCode);
    }

    [Fact]
    public void Money_Can_Convert()
    {
        var money = new Money("USD", "United States Dollars", Decimal.One);
        var result = money.ConvertToUSD(1);
        Assert.Equal(result, money);
    }
}