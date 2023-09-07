namespace CurrencyConversion.ConversionRepository.CsvConversionRepository;

public interface ICurrencyConversionCsvParser
{
    /// <summary>
    /// Open the given file and build a new repository
    /// </summary>
    /// <param name="filename">Filename to parse</param>
    /// <returns><see cref="InMemoryConversionRepository"/> to query</returns>
    Task<ICurrencyConversionRepository> LoadIntoRepositoryAsync(string filename);

    /// <summary>
    /// Read from the given stream and build a new repository
    /// </summary>
    /// <param name="stream"><see cref="TextReader"/> to parse</param>
    /// <returns><see cref="InMemoryConversionRepository"/> to query</returns>
    Task<ICurrencyConversionRepository> LoadIntoRepositoryAsync(TextReader stream);
}