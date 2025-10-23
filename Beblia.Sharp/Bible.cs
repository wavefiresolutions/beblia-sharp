using System.Collections.Generic;
using System.Linq;

namespace Beblia.Sharp
{
    /// <summary>
    /// Represents the entire Bible collection, containing testaments.
    /// Provides helper methods to query for specific verses, chapters, or books.
    /// </summary>
    public class Bible
    {
        /// <summary>
        /// The translation name, e.g., "English KJV".
        /// </summary>
        public string Translation { get; set; }
        
        /// <summary>
        /// The status of the translation, e.g., "Public Domain".
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// A list of testaments (e.g., "Old" and "New").
        /// </summary>
        public List<Testament> Testaments { get; set; }

        public Bible()
        {
            Testaments = new List<Testament>();
        }

        /// <summary>
        /// Gets a specific book by its global number.
        /// </summary>
        /// <param name="bookNumber">The book number (e.g., 1 for Genesis, 40 for Matthew).</param>
        /// <returns>The Book object, or null if not found.</returns>
        public Book? GetBook(int bookNumber)
        {
            // Search all testaments and flatten the list of books
            return Testaments.SelectMany(t => t.Books)
                             .FirstOrDefault(b => b.Number == bookNumber);
        }

        /// <summary>
        /// Gets a specific chapter by its book and chapter number.
        /// </summary>
        /// <param name="bookNumber">The book number.</param>
        /// <param name="chapterNumber">The chapter number.</param>
        /// <returns>The Chapter object, or null if not found.</returns>
        public Chapter? GetChapter(int bookNumber, int chapterNumber)
        {
            Book? book = GetBook(bookNumber);
            return book?.Chapters.FirstOrDefault(c => c.Number == chapterNumber);
        }

        /// <summary>
        /// Gets a specific verse by its book, chapter, and verse number.
        /// </summary>
        /// <param name="bookNumber">The book number.</param>
        /// <param name="chapterNumber">The chapter number.</param>
        /// <param name="verseNumber">The verse number.</param>
        /// <returns>The Verse object, or null if not found.</returns>
        public Verse? GetVerse(int bookNumber, int chapterNumber, int verseNumber)
        {
            Chapter? chapter = GetChapter(bookNumber, chapterNumber);
            return chapter?.Verses.FirstOrDefault(v => v.Number == verseNumber);
        }
    }
}