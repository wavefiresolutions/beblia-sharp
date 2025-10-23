# Beblia.Sharp

A C# library for parsing and loading Bible data from XML and binary formats.

## Features

- **XML Support**: Load Bible data from XML files
- **Binary Format**: Efficient binary format (.beblia) for faster loading and smaller file size
- **Format Auto-Detection**: Automatically detects whether a file is XML or binary
- **Query Methods**: Easy-to-use methods to query verses, chapters, and books
- **Book Name Support**: Query using book names (e.g., "Genesis", "Matthew")
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

// Get verse by book name
var verse = bible.GetVerse("Genesis", 1, 1);
Console.WriteLine(verse?.Text);

// Get a chapter
var chapter = bible.GetChapter("John", 3);

// Get a book
var book = bible.GetBook("Romans");
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

See the LICENSE file for details.
