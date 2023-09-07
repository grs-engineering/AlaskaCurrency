# CSV Converter

This subsystem will watch a given directory, read CSV files, and load them into an in-memory repo.

## CurrencyConversionCsvParser
Pretty straightforard.
Uses CsvHelper.

## FileSystemWatcherWrapper
This wraps the .net `System.IO.FileSystem.FileSystemWatcher` class to make it mockable.

## CurrencyDirectoryWatcher
Injected with a parser and watcher.
When the watcher triggers the `Change` event, it will use the parser to build a new repo.

## SelfReloadingConversionRepository
Wraps the watcher and delivers the current repo.
If the repo hasn't loaded yet, it will force a reload.
