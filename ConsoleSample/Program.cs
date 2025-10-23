// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Beblia.Sharp;

var bible = BibleParser.Load("EnglishKJBible.xml");
var verse = bible.GetVerse(1, 1, 1);
Console.WriteLine(verse.Text);
File.WriteAllText("KJV.json", JsonSerializer.Serialize(bible));