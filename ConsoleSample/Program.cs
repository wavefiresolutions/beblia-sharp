// See https://aka.ms/new-console-template for more information

using System;
using System.Text.Json;
using Beblia.Sharp;

Console.WriteLine("Beblia.Sharp Sample - Loading XML and Binary Formats\n");

// Test 1: Load from XML
Console.WriteLine("Test 1: Loading from XML file...");
var bibleXml = BibleParser.Load("EnglishKJBible.xml");
var verse1 = bibleXml.GetVerse(1, 1, 1);
Console.WriteLine($"Genesis 1:1 (from XML): {verse1?.Text ?? "Verse not found"}");

// Test 2: Load from Binary
Console.WriteLine("\nTest 2: Loading from binary file...");
var bibleBinary = BibleParser.Load("EnglishKJBible.beblia");
var verse2 = bibleBinary.GetVerse(1, 1, 1);
Console.WriteLine($"Genesis 1:1 (from Binary): {verse2?.Text ?? "Verse not found"}");

// Test 3: Verify they are the same
Console.WriteLine("\nTest 3: Comparing XML and Binary loaded data...");
bool same = verse1?.Text == verse2?.Text;
Console.WriteLine($"Verse texts match: {same}");

// Test 4: Get verse by book name (case-insensitive)
var verse3 = bibleBinary.GetVerse("Genesis", 1, 1);
Console.WriteLine($"\nGenesis 1:1 (by name): {verse3?.Text ?? "Verse not found"}");

// Test 5: Get verse by abbreviation (case-insensitive)
var verse4 = bibleBinary.GetVerse("Gen", 1, 1);
Console.WriteLine($"Genesis 1:1 (by abbreviation): {verse4?.Text ?? "Verse not found"}");

// Test 6: Testament enum
Console.WriteLine($"\n=== Testament Enum ===");
Console.WriteLine($"Testament.Old = {Testament.Old}");
Console.WriteLine($"Testament.New = {Testament.New}");

// Test 7: GetBooks() methods
Console.WriteLine($"\n=== GetBooks() Methods ===");
var allBooks = bibleBinary.GetBooks();
Console.WriteLine($"Total books: {allBooks.Count}");
var oldTestamentBooks = bibleBinary.GetBooks(Testament.Old);
Console.WriteLine($"Old Testament books: {oldTestamentBooks.Count}");
var newTestamentBooks = bibleBinary.GetBooks(Testament.New);
Console.WriteLine($"New Testament books: {newTestamentBooks.Count}");

// Test 8: Book info
Console.WriteLine($"\n=== Book Information ===");
var genesis = bibleBinary.GetBook(1);
Console.WriteLine($"Book 1: {genesis?.Name} ({genesis?.Abbreviation})");
var john = bibleBinary.GetBook(43);
Console.WriteLine($"Book 43: {john?.Name} ({john?.Abbreviation})");

// Test 9: Quick search
Console.WriteLine($"\n=== Quick Search ===");
var john316 = bibleBinary.Get("JN 3:16");
Console.WriteLine($"JN 3:16: {john316[0].Text}");

var john31_2 = bibleBinary.Get("JOHN 3:1-2");
Console.WriteLine($"\nJOHN 3:1-2 ({john31_2.Count} verses):");
foreach (var verse in john31_2)
{
    Console.WriteLine($"  Verse {verse.Number}: {verse.Text?.Substring(0, Math.Min(60, verse.Text.Length))}...");
}

// Test 10: Book name lookup
Console.WriteLine($"\n=== Localization ===");
Console.WriteLine($"Book 40 is: {bibleBinary.Localization.GetBookName(40)}");
Console.WriteLine($"Matthew is book number: {bibleBinary.Localization.GetBookNumber("Matthew")}");
Console.WriteLine($"Case-insensitive: 'matthew' = {bibleBinary.Localization.GetBookNumber("matthew")}");
Console.WriteLine($"Abbreviation: 'Mat' = {bibleBinary.Localization.GetBookNumber("Mat")}");

Console.WriteLine("\n✓ All tests completed successfully!");