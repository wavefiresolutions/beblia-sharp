// See https://aka.ms/new-console-template for more information

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

// Test 4: Get verse by book name (new overload)
var verse3 = bibleBinary.GetVerse("Genesis", 1, 1);
Console.WriteLine($"\nGenesis 1:1 (by name): {verse3?.Text ?? "Verse not found"}");

// Test 5: Book name lookup
Console.WriteLine($"\nBook 40 is: {Localization.GetBookName(40)}");
Console.WriteLine($"Matthew is book number: {Localization.GetBookNumber("Matthew")}");

// Test 6: Save to JSON for comparison
Console.WriteLine("\nTest 6: Saving binary-loaded Bible to JSON...");
File.WriteAllText("KJV.json", JsonSerializer.Serialize(bibleBinary));
Console.WriteLine("Saved to KJV.json");

Console.WriteLine("\n✓ All tests completed successfully!");