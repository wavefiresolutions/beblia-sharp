using System;
using Beblia.Sharp;

Console.WriteLine("=== Testing New Beblia.Sharp Features ===\n");

var bible = BibleParser.Load("EnglishKJBible.beblia");

// Test 1: Testament Enum
Console.WriteLine("Test 1: Testament Enum");
Console.WriteLine($"Testament.Old = {Testament.Old}");
Console.WriteLine($"Testament.New = {Testament.New}");
Console.WriteLine();

// Test 2: GetBooks() - all books
Console.WriteLine("Test 2: GetBooks() - Get all books");
var allBooks = bible.GetBooks();
Console.WriteLine($"Total books: {allBooks.Count}");
Console.WriteLine($"First book: {allBooks[0].Number} - {allBooks[0].Name} ({allBooks[0].Abbreviation})");
Console.WriteLine($"Last book: {allBooks[allBooks.Count - 1].Number} - {allBooks[allBooks.Count - 1].Name} ({allBooks[allBooks.Count - 1].Abbreviation})");
Console.WriteLine();

// Test 3: GetBooks(Testament) - Old Testament books
Console.WriteLine("Test 3: GetBooks(Testament.Old)");
var oldTestamentBooks = bible.GetBooks(Testament.Old);
Console.WriteLine($"Old Testament books: {oldTestamentBooks.Count}");
Console.WriteLine($"First OT book: {oldTestamentBooks[0].Number} - {oldTestamentBooks[0].Name}");
Console.WriteLine($"Last OT book: {oldTestamentBooks[oldTestamentBooks.Count - 1].Number} - {oldTestamentBooks[oldTestamentBooks.Count - 1].Name}");
Console.WriteLine();

// Test 4: GetBooks(Testament) - New Testament books
Console.WriteLine("Test 4: GetBooks(Testament.New)");
var newTestamentBooks = bible.GetBooks(Testament.New);
Console.WriteLine($"New Testament books: {newTestamentBooks.Count}");
Console.WriteLine($"First NT book: {newTestamentBooks[0].Number} - {newTestamentBooks[0].Name}");
Console.WriteLine($"Last NT book: {newTestamentBooks[newTestamentBooks.Count - 1].Number} - {newTestamentBooks[newTestamentBooks.Count - 1].Name}");
Console.WriteLine();

// Test 5: GetChapters(Book)
Console.WriteLine("Test 5: GetChapters(Book) - Genesis chapters");
var genesis = bible.GetBook(1);
if (genesis != null)
{
    var chapters = bible.GetChapters(genesis);
    Console.WriteLine($"Genesis has {chapters.Count} chapters");
    Console.WriteLine($"First chapter: {chapters[0].Number}");
    Console.WriteLine($"Last chapter: {chapters[chapters.Count - 1].Number}");
}
Console.WriteLine();

// Test 6: GetVerses(Chapter)
Console.WriteLine("Test 6: GetVerses(Chapter) - Genesis 1 verses");
var genesis1 = bible.GetChapter(1, 1);
if (genesis1 != null)
{
    var verses = bible.GetVerses(genesis1);
    Console.WriteLine($"Genesis 1 has {verses.Count} verses");
    Console.WriteLine($"First verse: {verses[0].Number} - {verses[0].Text?.Substring(0, Math.Min(50, verses[0].Text.Length))}...");
}
Console.WriteLine();

// Test 7: Abbreviated names - case insensitive
Console.WriteLine("Test 7: Abbreviated book names (case-insensitive)");
Console.WriteLine($"Localization.GetBookNumber(\"Gen\") = {Localization.GetBookNumber("Gen")}");
Console.WriteLine($"Localization.GetBookNumber(\"gen\") = {Localization.GetBookNumber("gen")}");
Console.WriteLine($"Localization.GetBookNumber(\"GEN\") = {Localization.GetBookNumber("GEN")}");
Console.WriteLine($"Localization.GetBookNumber(\"Jn\") = {Localization.GetBookNumber("Jn")}");
Console.WriteLine($"Localization.GetBookNumber(\"jn\") = {Localization.GetBookNumber("jn")}");
Console.WriteLine($"Localization.GetBookNumber(\"JN\") = {Localization.GetBookNumber("JN")}");
Console.WriteLine();

// Test 8: Quick search - single verse
Console.WriteLine("Test 8: Get(\"JN 3:16\") - Single verse");
var john316 = bible.Get("JN 3:16");
Console.WriteLine($"Found {john316.Count} verse(s)");
if (john316.Count > 0)
{
    Console.WriteLine($"John 3:16 - {john316[0].Text}");
}
Console.WriteLine();

// Test 9: Quick search - verse range
Console.WriteLine("Test 9: Get(\"JOHN 3:1-2\") - Verse range");
var john31_2 = bible.Get("JOHN 3:1-2");
Console.WriteLine($"Found {john31_2.Count} verse(s)");
foreach (var verse in john31_2)
{
    Console.WriteLine($"John 3:{verse.Number} - {verse.Text?.Substring(0, Math.Min(50, verse.Text.Length))}...");
}
Console.WriteLine();

// Test 10: Quick search - multiple verses and ranges
Console.WriteLine("Test 10: Get(\"JN 3:1,4,5-6\") - Multiple verses and ranges");
var john3mixed = bible.Get("JN 3:1,4,5-6");
Console.WriteLine($"Found {john3mixed.Count} verse(s)");
foreach (var verse in john3mixed)
{
    Console.WriteLine($"John 3:{verse.Number} - {verse.Text?.Substring(0, Math.Min(40, verse.Text.Length))}...");
}
Console.WriteLine();

// Test 11: Case insensitive book names
Console.WriteLine("Test 11: Case-insensitive book names");
var genesisLower = bible.GetBook("genesis");
var genesisUpper = bible.GetBook("GENESIS");
var genesisMixed = bible.GetBook("GeNeSiS");
Console.WriteLine($"genesis: {genesisLower?.Name}");
Console.WriteLine($"GENESIS: {genesisUpper?.Name}");
Console.WriteLine($"GeNeSiS: {genesisMixed?.Name}");
Console.WriteLine();

// Test 12: Book abbreviations
Console.WriteLine("Test 12: Book abbreviations");
Console.WriteLine($"Book 1 abbreviation: {Localization.GetBookAbbreviation(1)}");
Console.WriteLine($"Book 43 abbreviation: {Localization.GetBookAbbreviation(43)}");
Console.WriteLine($"Book 40 abbreviation: {Localization.GetBookAbbreviation(40)}");
Console.WriteLine();

Console.WriteLine("✓ All tests completed!");
