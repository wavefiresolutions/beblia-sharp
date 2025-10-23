using System.Collections.Generic;
using System.IO;
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
        public string? Translation { get; set; }
        
        /// <summary>
        /// The status of the translation, e.g., "Public Domain".
        /// </summary>
        public string? Status { get; set; }
        
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

        /// <summary>
        /// Gets a specific book by its name.
        /// </summary>
        /// <param name="bookName">The name of the book (e.g., "Genesis", "Matthew").</param>
        /// <returns>The Book object, or null if not found.</returns>
        public Book? GetBook(string bookName)
        {
            int? bookNumber = Localization.GetBookNumber(bookName);
            if (bookNumber.HasValue)
            {
                return GetBook(bookNumber.Value);
            }
            return null;
        }

        /// <summary>
        /// Gets a specific chapter by its book name and chapter number.
        /// </summary>
        /// <param name="bookName">The name of the book.</param>
        /// <param name="chapterNumber">The chapter number.</param>
        /// <returns>The Chapter object, or null if not found.</returns>
        public Chapter? GetChapter(string bookName, int chapterNumber)
        {
            int? bookNumber = Localization.GetBookNumber(bookName);
            if (bookNumber.HasValue)
            {
                return GetChapter(bookNumber.Value, chapterNumber);
            }
            return null;
        }

        /// <summary>
        /// Gets a specific verse by its book name, chapter, and verse number.
        /// </summary>
        /// <param name="bookName">The name of the book (e.g., "Genesis", "Matthew").</param>
        /// <param name="chapterNumber">The chapter number.</param>
        /// <param name="verseNumber">The verse number.</param>
        /// <returns>The Verse object, or null if not found.</returns>
        public Verse? GetVerse(string bookName, int chapterNumber, int verseNumber)
        {
            int? bookNumber = Localization.GetBookNumber(bookName);
            if (bookNumber.HasValue)
            {
                return GetVerse(bookNumber.Value, chapterNumber, verseNumber);
            }
            return null;
        }

        /// <summary>
        /// Saves the Bible to a binary file (.beblia format).
        /// </summary>
        /// <param name="filePath">The path where the binary file will be saved.</param>
        public void SaveBinary(string filePath)
        {
            using (FileStream stream = File.Create(filePath))
            {
                SaveBinary(stream);
            }
        }

        /// <summary>
        /// Saves the Bible to a binary stream (.beblia format).
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void SaveBinary(Stream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true))
            {
                // Write a magic number/header to identify .beblia files
                writer.Write("BEBLIA");
                writer.Write((byte)1); // Version number
                
                // Write Translation and Status
                writer.Write(Translation ?? string.Empty);
                writer.Write(Status ?? string.Empty);
                
                // Write number of testaments
                writer.Write(Testaments.Count);
                
                foreach (Testament testament in Testaments)
                {
                    writer.Write(testament.Name ?? string.Empty);
                    writer.Write(testament.Books.Count);
                    
                    foreach (Book book in testament.Books)
                    {
                        writer.Write(book.Number);
                        writer.Write(book.Chapters.Count);
                        
                        foreach (Chapter chapter in book.Chapters)
                        {
                            writer.Write(chapter.Number);
                            writer.Write(chapter.Verses.Count);
                            
                            foreach (Verse verse in chapter.Verses)
                            {
                                writer.Write(verse.Number);
                                writer.Write(verse.Text ?? string.Empty);
                            }
                        }
                    }
                }
            }
        }
    }
}