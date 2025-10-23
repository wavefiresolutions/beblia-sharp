# Beblia.Sharp

A C# library for parsing and loading Bible data from XML and binary formats.

## Features

- **XML Support**: Load Bible data from XML files
- **Binary Format**: Efficient binary format (.beblia) for faster loading and smaller file size
- **Format Auto-Detection**: Automatically detects whether a file is XML or binary
- **Query Methods**: Easy-to-use methods to query verses, chapters, and books
- **Book Name Support**: Query using book names (e.g., "Genesis", "Matthew") or abbreviations (e.g., "Gen", "Jn") - case-insensitive
- **Testament Enum**: Testament type is now an enum (Old/New)
- **Quick Search**: Search for verses using natural reference formats like "JN 3:16" or "JOHN 3:1-5"
- **Editable Localization**: Load custom book names and abbreviations from a text file
- **Converter Tool**: Command-line tool to convert XML Bibles to binary format

## Projects

### Beblia.Sharp
The core library that provides Bible parsing and loading functionality.

### Beblia.Converter
A command-line tool to convert XML Bible files to the binary .beblia format.

### ConsoleSample
A sample application demonstrating how to use the library.

## Usage

### Loading a Bible (XML or Binary)

```csharp
using Beblia.Sharp;

// Load from XML
var bible = BibleParser.Load("EnglishKJV.xml");

// Load from binary - same API!
var bible = BibleParser.Load("EnglishKJV.beblia");

// The format is auto-detected
```

### Querying Verses

```csharp
// Get verse by book number
var verse = bible.GetVerse(1, 1, 1); // Genesis 1:1
Console.WriteLine(verse?.Text);

// Get verse by book name (case-insensitive)
var verse = bible.GetVerse("Genesis", 1, 1);
var verse2 = bible.GetVerse("genesis", 1, 1); // Also works
Console.WriteLine(verse?.Text);

// Get verse by abbreviation (case-insensitive)
var verse3 = bible.GetVerse("Gen", 1, 1);
var verse4 = bible.GetVerse("GEN", 1, 1); // Also works

// Quick search with reference string
var verses = bible.Get("JN 3:16"); // Single verse
var verses2 = bible.Get("JOHN 3:1-2"); // Verse range
var verses3 = bible.Get("JN 3:1,4,5-6"); // Multiple verses and ranges

// Get a chapter
var chapter = bible.GetChapter("John", 3);

// Get a book
var book = bible.GetBook("Romans");
```

### Getting Books, Chapters, and Verses

```csharp
// Get all books in the Bible (returns list without nested chapters/verses)
var allBooks = bible.GetBooks();
Console.WriteLine($"Total books: {allBooks.Count}");
foreach (var book in allBooks)
{
    Console.WriteLine($"{book.Number}. {book.Name} ({book.Abbreviation})");
}

// Get books by testament
var oldTestamentBooks = bible.GetBooks(Testament.Old);
var newTestamentBooks = bible.GetBooks(Testament.New);

// Get all chapters in a book (returns list without nested verses)
var genesis = bible.GetBook(1);
var chapters = bible.GetChapters(genesis);

// Get all verses in a chapter (with text)
var genesis1 = bible.GetChapter(1, 1);
var verses = bible.GetVerses(genesis1);
```

### Custom Localization

You can customize book names and abbreviations by loading a localization file:

```csharp
// Load custom localization from file
Localization.LoadFromFile("localization.txt");
```

Localization file format (one book per line):
```
# Format: number abbreviation fullname
1 Gen Genesis
2 Exo Exodus
...
43 Joh John
...
```

### Saving to Binary Format

```csharp
// Load from XML
var bible = BibleParser.Load("EnglishKJV.xml");

// Save to binary format
bible.SaveBinary("EnglishKJV.beblia");
```

### Using the Converter Tool

Convert a single XML file to binary:
```bash
dotnet run --project Beblia.Converter -- EnglishKJV.xml
# Creates EnglishKJV.beblia
```

Specify output filename:
```bash
dotnet run --project Beblia.Converter -- EnglishKJV.xml KJV.beblia
```

Convert multiple files:
```bash
dotnet run --project Beblia.Converter -- Bible1.xml Bible2.xml Bible3.xml
# Creates Bible1.beblia, Bible2.beblia, Bible3.beblia
```

## Binary Format (.beblia)

The binary format offers several advantages:
- **Smaller file size**: Typically 15-20% smaller than XML
- **Faster loading**: Binary parsing is faster than XML parsing
- **Same functionality**: All API methods work identically with both formats

The format includes:
- Magic header for format detection
- Version number for future compatibility
- Length-prefixed strings for efficient storage
- Nested structure preserving testament → book → chapter → verse hierarchy

## Building

```bash
dotnet build
```

## Running the Sample

```bash
dotnet run --project ConsoleSample
```

## License

This is free and unencumbered software released into the public domain. See the UNLICENSE file for details.
