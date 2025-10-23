using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Beblia.Sharp
{
    /// <summary>
    /// Provides localization mappings between book names and numbers.
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// Maps book names (case-insensitive) to their corresponding book numbers.
        /// </summary>
        private static Dictionary<string, int> _bookNames = new Dictionary<string, int>();

        /// <summary>
        /// Maps book names to abbreviated names (case-insensitive).
        /// </summary>
        private static Dictionary<string, string> _abbreviations = new Dictionary<string, string>();

        /// <summary>
        /// Reverse lookup dictionary for mapping book numbers to names.
        /// </summary>
        private static Dictionary<int, string> _bookNumbers = new Dictionary<int, string>();

        /// <summary>
        /// Maps book numbers to abbreviated names.
        /// </summary>
        private static Dictionary<int, string> _bookAbbreviations = new Dictionary<int, string>();

        static Localization()
        {
            LoadDefaultLocalization();
        }

        /// <summary>
        /// Loads the default localization data.
        /// </summary>
        private static void LoadDefaultLocalization()
        {
            _bookNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                // Old Testament
                { "Genesis", 1 },
                { "Exodus", 2 },
                { "Leviticus", 3 },
                { "Numbers", 4 },
                { "Deuteronomy", 5 },
                { "Joshua", 6 },
                { "Judges", 7 },
                { "Ruth", 8 },
                { "1 Samuel", 9 },
                { "2 Samuel", 10 },
                { "1 Kings", 11 },
                { "2 Kings", 12 },
                { "1 Chronicles", 13 },
                { "2 Chronicles", 14 },
                { "Ezra", 15 },
                { "Nehemiah", 16 },
                { "Esther", 17 },
                { "Job", 18 },
                { "Psalms", 19 },
                { "Proverbs", 20 },
                { "Ecclesiastes", 21 },
                { "Song of Solomon", 22 },
                { "Isaiah", 23 },
                { "Jeremiah", 24 },
                { "Lamentations", 25 },
                { "Ezekiel", 26 },
                { "Daniel", 27 },
                { "Hosea", 28 },
                { "Joel", 29 },
                { "Amos", 30 },
                { "Obadiah", 31 },
                { "Jonah", 32 },
                { "Micah", 33 },
                { "Nahum", 34 },
                { "Habakkuk", 35 },
                { "Zephaniah", 36 },
                { "Haggai", 37 },
                { "Zechariah", 38 },
                { "Malachi", 39 },

                // New Testament
                { "Matthew", 40 },
                { "Mark", 41 },
                { "Luke", 42 },
                { "John", 43 },
                { "Acts", 44 },
                { "Romans", 45 },
                { "1 Corinthians", 46 },
                { "2 Corinthians", 47 },
                { "Galatians", 48 },
                { "Ephesians", 49 },
                { "Philippians", 50 },
                { "Colossians", 51 },
                { "1 Thessalonians", 52 },
                { "2 Thessalonians", 53 },
                { "1 Timothy", 54 },
                { "2 Timothy", 55 },
                { "Titus", 56 },
                { "Philemon", 57 },
                { "Hebrews", 58 },
                { "James", 59 },
                { "1 Peter", 60 },
                { "2 Peter", 61 },
                { "1 John", 62 },
                { "2 John", 63 },
                { "3 John", 64 },
                { "Jude", 65 },
                { "Revelation", 66 }
            };

            // Add abbreviations
            _abbreviations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Old Testament
                { "Gen", "Genesis" },
                { "Exo", "Exodus" },
                { "Lev", "Leviticus" },
                { "Num", "Numbers" },
                { "Deu", "Deuteronomy" },
                { "Jos", "Joshua" },
                { "Jdg", "Judges" },
                { "Rut", "Ruth" },
                { "1Sa", "1 Samuel" },
                { "2Sa", "2 Samuel" },
                { "1Ki", "1 Kings" },
                { "2Ki", "2 Kings" },
                { "1Ch", "1 Chronicles" },
                { "2Ch", "2 Chronicles" },
                { "Ezr", "Ezra" },
                { "Neh", "Nehemiah" },
                { "Est", "Esther" },
                { "Job", "Job" },
                { "Psa", "Psalms" },
                { "Pro", "Proverbs" },
                { "Ecc", "Ecclesiastes" },
                { "Sng", "Song of Solomon" },
                { "Isa", "Isaiah" },
                { "Jer", "Jeremiah" },
                { "Lam", "Lamentations" },
                { "Eze", "Ezekiel" },
                { "Dan", "Daniel" },
                { "Hos", "Hosea" },
                { "Joe", "Joel" },
                { "Amo", "Amos" },
                { "Oba", "Obadiah" },
                { "Jon", "Jonah" },
                { "Mic", "Micah" },
                { "Nah", "Nahum" },
                { "Hab", "Habakkuk" },
                { "Zep", "Zephaniah" },
                { "Hag", "Haggai" },
                { "Zec", "Zechariah" },
                { "Mal", "Malachi" },

                // New Testament
                { "Mat", "Matthew" },
                { "Mrk", "Mark" },
                { "Luk", "Luke" },
                { "Joh", "John" },
                { "Jn", "John" },
                { "Act", "Acts" },
                { "Rom", "Romans" },
                { "1Co", "1 Corinthians" },
                { "2Co", "2 Corinthians" },
                { "Gal", "Galatians" },
                { "Eph", "Ephesians" },
                { "Phi", "Philippians" },
                { "Col", "Colossians" },
                { "1Th", "1 Thessalonians" },
                { "2Th", "2 Thessalonians" },
                { "1Ti", "1 Timothy" },
                { "2Ti", "2 Timothy" },
                { "Tit", "Titus" },
                { "Phm", "Philemon" },
                { "Heb", "Hebrews" },
                { "Jam", "James" },
                { "1Pe", "1 Peter" },
                { "2Pe", "2 Peter" },
                { "1Jo", "1 John" },
                { "2Jo", "2 John" },
                { "3Jo", "3 John" },
                { "Jud", "Jude" },
                { "Rev", "Revelation" }
            };

            // Add all abbreviations to the bookNames dictionary
            foreach (var abbrev in _abbreviations)
            {
                if (!_bookNames.ContainsKey(abbrev.Key))
                {
                    _bookNames[abbrev.Key] = _bookNames[abbrev.Value];
                }
            }

            // Build _bookNumbers (number to full name) - using GroupBy to handle duplicates
            _bookNumbers = _bookNames
                .Where(kvp => !_abbreviations.ContainsKey(kvp.Key))
                .GroupBy(kvp => kvp.Value)
                .ToDictionary(g => g.Key, g => g.First().Key);

            // Build _bookAbbreviations (number to abbreviation) - using GroupBy to handle duplicates
            _bookAbbreviations = _abbreviations
                .Where(kvp => _bookNames.ContainsKey(kvp.Value))
                .GroupBy(kvp => _bookNames[kvp.Value])
                .ToDictionary(g => g.Key, g => g.First().Key);
        }

        /// <summary>
        /// Loads localization from a text file.
        /// Format: number abbreviation fullname (e.g., "1 Gen Genesis")
        /// </summary>
        /// <param name="filePath">Path to the localization file.</param>
        public static void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Localization file not found.", filePath);
            }

            _bookNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _abbreviations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _bookNumbers = new Dictionary<int, string>();
            _bookAbbreviations = new Dictionary<int, string>();

            foreach (string line in File.ReadAllLines(filePath))
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                {
                    continue; // Skip empty lines and comments
                }

                string[] parts = trimmedLine.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3 && int.TryParse(parts[0], out int bookNumber))
                {
                    string abbreviation = parts[1];
                    string fullName = parts[2];

                    _bookNames[fullName] = bookNumber;
                    _bookNames[abbreviation] = bookNumber;
                    _abbreviations[abbreviation] = fullName;
                    _bookNumbers[bookNumber] = fullName;
                    _bookAbbreviations[bookNumber] = abbreviation;
                }
            }
        }

        /// <summary>
        /// Gets the book number for a given book name or abbreviation (case-insensitive).
        /// </summary>
        /// <param name="bookName">The name or abbreviation of the book.</param>
        /// <returns>The book number, or null if not found.</returns>
        public static int? GetBookNumber(string bookName)
        {
            if (_bookNames.TryGetValue(bookName, out int number))
            {
                return number;
            }
            return null;
        }

        /// <summary>
        /// Gets the book name for a given book number.
        /// </summary>
        /// <param name="bookNumber">The book number.</param>
        /// <returns>The book name, or null if not found.</returns>
        public static string? GetBookName(int bookNumber)
        {
            if (_bookNumbers.TryGetValue(bookNumber, out string? name))
            {
                return name;
            }
            return null;
        }

        /// <summary>
        /// Gets the abbreviated book name for a given book number.
        /// </summary>
        /// <param name="bookNumber">The book number.</param>
        /// <returns>The abbreviated book name, or null if not found.</returns>
        public static string? GetBookAbbreviation(int bookNumber)
        {
            if (_bookAbbreviations.TryGetValue(bookNumber, out string? abbreviation))
            {
                return abbreviation;
            }
            return null;
        }
    }
}