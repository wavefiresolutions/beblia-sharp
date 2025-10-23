using System.Collections.Generic;
using System.Linq;

namespace Beblia.Sharp
{
    /// <summary>
    /// Provides localization mappings between book names and numbers.
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// Maps book names to their corresponding book numbers.
        /// </summary>
        public static readonly Dictionary<string, int> BookNames = new Dictionary<string, int>
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

        /// <summary>
        /// Reverse lookup dictionary for mapping book numbers to names.
        /// </summary>
        private static readonly Dictionary<int, string> BookNumbers = BookNames.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        /// <summary>
        /// Gets the book number for a given book name.
        /// </summary>
        /// <param name="bookName">The name of the book.</param>
        /// <returns>The book number, or null if not found.</returns>
        public static int? GetBookNumber(string bookName)
        {
            if (BookNames.TryGetValue(bookName, out int number))
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
            if (BookNumbers.TryGetValue(bookNumber, out string? name))
            {
                return name;
            }
            return null;
        }
    }
}

