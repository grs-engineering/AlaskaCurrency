using CurrencyConversion;
using CurrencyConversion.ConversionRepository;
using CurrencyConversion.ConversionRepository.CsvConversionRepository;
using Moq;

namespace CurrencyTests;

public class CurrencyDirectoryWatcherTests
{
    [Fact]
    public void Watcher_BuildsNewRepo_OnChangedEvent()
    {
        var mockWatcher = new Mock<IDirectoryChangedWatcher>();
        var mockParser = new Mock<ICurrencyConversionCsvParser>();
        var dummyRepo = Mock.Of<ICurrencyConversionRepository>();

        // When the parser is told to load, we return the dummy
        mockParser
            .Setup(p =>
                p.LoadIntoRepositoryAsync(It.IsAny<string>()))
            .ReturnsAsync(dummyRepo);

        var cdw = new CurrencyDirectoryWatcher(mockWatcher.Object, mockParser.Object, string.Empty);
        var then = cdw.LastUpdated;
        
        // Trigger the file event with the given file event
        mockWatcher.Raise(w =>
            w.Changed += null, // kind of wierd syntax, but that's how it goes
            new FileSystemEventArgs(WatcherChangeTypes.Changed, "dummy/dir", "Currency_test.csv"));
        
        mockParser.Verify(p => p.LoadIntoRepositoryAsync(
                It.Is("dummy/dir/Currency_test.csv", StringComparer.Ordinal)));
        
        // Ensure that the timestamp was updated
        Assert.True(then < cdw.LastUpdated);
        
        // Ensure that it's using the dummy we just gave it
        Assert.Same(dummyRepo, cdw.CurrentRepository);
    }
    
    [Fact(Skip = "This is an especially hacky test, but it helps debug")]
    public async Task Watcher_LoadsFromFile()
    {
        var mockWatcher = new Mock<IDirectoryChangedWatcher>();
        var parser = new CurrencyConversionCsvParser();
     
        
        var cdw = new CurrencyDirectoryWatcher(mockWatcher.Object, parser, "../../../TestData");
        await cdw.LoadCurrent();
        
        Assert.NotNull(cdw.CurrentRepository);
    }
}