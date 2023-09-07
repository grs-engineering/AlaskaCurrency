namespace CurrencyConversion.Money;

public class Currency : IEquatable<Currency>
{
    public string CountryCode { get; }
    public string CurrencyName { get; }

    /// <summary>
    /// Creates a new <see cref="Currency"/> object
    /// </summary>
    /// <param name="countryCode">Country Code (e.g. USD)</param>
    /// <param name="currencyName">Currency Name (e.g. Mexican Pesos)</param>
    public Currency(string countryCode, string? currencyName = null)
    {
        countryCode = countryCode.Trim();
        if (countryCode == string.Empty)
        {
            throw new ArgumentOutOfRangeException(
                nameof(countryCode),
                "Country code cannot be empty.");
        }
        
        CountryCode = countryCode.ToUpperInvariant();
        CurrencyName = currencyName;
    }
        
    // Common values
    public static readonly Currency USD = new Currency("USD", "United States Dollar");
    public static readonly Currency GBP = new Currency("GBP", "Great British Pound");
    public static readonly Currency CAD = new Currency("CAD", "Canadian Dollar");
    public static readonly Currency MXN = new Currency("MXN", "Mexican Peso");

    public bool Equals(Currency? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(CountryCode, other.CountryCode, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Currency)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(CountryCode, StringComparer.OrdinalIgnoreCase);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(Currency? left, Currency? right) => Equals(left, right);

    public static bool operator !=(Currency? left, Currency? right) => !Equals(left, right);
}