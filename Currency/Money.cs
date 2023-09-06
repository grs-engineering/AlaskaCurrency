namespace Currency;

public class Money : IEquatable<Money>
{
    public string CountryCode { get; init; }
    public string CurrencyName { get; init; }
    public decimal ConversionToUSD { get; init; }

    /// <summary>
    /// Creates a new Money object
    /// </summary>
    /// <param name="countryCode"></param>
    /// <param name="currencyName"></param>
    /// <param name="conversionToUsd"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Money(string countryCode, string currencyName, decimal conversionToUsd)
    {
        CountryCode = countryCode.Trim().ToUpperInvariant();
        CurrencyName = currencyName;
        ConversionToUSD = conversionToUsd > 0
            ? conversionToUsd
            : throw new ArgumentOutOfRangeException(nameof(conversionToUsd), conversionToUsd,
                "Conversion rate must be greater than 0.");
    }

    public Money ConvertToUSD(decimal amount) =>
        new Money("USD", "Unites States Dollars", ConversionToUSD * amount);

    public bool Equals(Money? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return CountryCode == other.CountryCode && ConversionToUSD == other.ConversionToUSD;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Money)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CountryCode, ConversionToUSD);
    }
}