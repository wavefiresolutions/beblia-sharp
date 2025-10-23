// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Beblia.Sharp;

var bible = BibleParser.Load("EnglishKJBible.xml");

// Get verse by book number (original method)
var verse1 = bible.GetVerse(1, 1, 1);
Console.WriteLine($"Genesis 1:1 (by number): {verse1?.Text ?? "Verse not found"}");

// Get verse by book name (new overload)
var verse2 = bible.GetVerse("Genesis", 1, 1);
Console.WriteLine($"Genesis 1:1 (by name): {verse2?.Text ?? "Verse not found"}");

// Demonstrate book name lookup
Console.WriteLine($"\nBook 40 is: {Localization.GetBookName(40)}");
Console.WriteLine($"Matthew is book number: {Localization.GetBookNumber("Matthew")}");

File.WriteAllText("KJV.json", JsonSerializer.Serialize(bible));