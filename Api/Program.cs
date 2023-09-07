using CurrencyConversion;
using CurrencyConversion.ConversionRepository;
using CurrencyConversion.ConversionRepository.CsvConversionRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// TODO: config
var watchDirectory = Path.Combine(builder.Environment.ContentRootPath, "Data");

// Setup directory watcher and parser
builder.Services
    .AddSingleton<ICurrencyConversionCsvParser, CurrencyConversionCsvParser>()
    .AddSingleton<FileSystemWatcher>(_ => new FileSystemWatcher(watchDirectory, "Conversion*.csv"))
    .AddSingleton<IDirectoryChangedWatcher, FileSystemWatcherWrapper>()
    .AddSingleton<ICurrencyDirectoryWatcher>(svc =>
    {
        var parser = svc.GetRequiredService<ICurrencyConversionCsvParser>();
        var watcher = svc.GetRequiredService<IDirectoryChangedWatcher>();

        return new CurrencyDirectoryWatcher(watcher, parser, watchDirectory);
    });

// Pull repo from directory watcher
builder.Services
    .AddTransient<ICurrencyConversionRepository>(svc =>
        new SelfReloadingConversionRepository(svc.GetRequiredService<ICurrencyDirectoryWatcher>()));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();