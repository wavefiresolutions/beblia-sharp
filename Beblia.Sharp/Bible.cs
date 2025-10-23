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
        /// A list of testaments (Old and New).
        /// </summary>
        public List<TestamentData> Testaments { get; set; }

        /// <summary>
        /// The localization data for book names and abbreviations.
        /// </summary>
        public Localization Localization { get; set; }

        public Bible()
        {
            Testaments = new List<TestamentData>();
            Localization = new Localization();
        }

        /// <summary>
        /// Gets all books in the Bible in order (no nested chapter/verse info).
        /// </summary>
        /// <returns>A list of all books.</returns>
        public List<Book> GetBooks()
        {
            var books = Testaments.SelectMany(t => t.Books).ToList();
            foreach (var book in books)
            {
                book.SetLocalization(Localization);
            }
            return books;
        }

        /// <summary>
        /// Gets all books in a specific testament (no nested chapter/verse info).
        /// </summary>
        /// <param name="testament">The testament (Old or New).</param>
        /// <returns>A list of books in the specified testament.</returns>
        public List<Book> GetBooks(Testament testament)
        {
            var books = Testaments
                .Where(t => t.Testament == testament)
                .SelectMany(t => t.Books)
                .ToList();
            foreach (var book in books)
            {
                book.SetLocalization(Localization);
            }
            return books;
        }

        /// <summary>
        /// Gets all chapters in a specific book (no nested verse info).
        /// </summary>
        /// <param name="book">The book.</param>
        /// <returns>A list of chapters in the book.</returns>
        public List<Chapter> GetChapters(Book book)
        {
            return book.Chapters;
        }

        /// <summary>
        /// Gets all verses in a specific chapter (with verse text).
        /// </summary>
        /// <param name="chapter">The chapter.</param>
        /// <returns>A list of verses in the chapter.</returns>
        public List<Verse> GetVerses(Chapter chapter)
        {
            return chapter.Verses;
        }

        /// <summary>
        /// Gets a specific book by its global number.
        /// </summary>
        /// <param name="bookNumber">The book number (e.g., 1 for Genesis, 40 for Matthew).</param>
        /// <returns>The Book object, or null if not found.</returns>
        public Book? GetBook(int bookNumber)
        {
            // Search all testaments and flatten the list of books
            var book = Testaments.SelectMany(t => t.Books)
                             .FirstOrDefault(b => b.Number == bookNumber);
            if (book != null)
            {
                book.SetLocalization(Localization);
            }
            return book;
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
        /// Gets one or more verses using a quick search string format.
        /// Supports formats like "JN 3:16", "JOHN 3:1-2", "JN 3:1,4,5-6".
        /// </summary>
        /// <param name="reference">The verse reference string (case-insensitive).</param>
        /// <returns>A list of verses matching the reference, or an empty list if not found.</returns>
        public List<Verse> Get(string reference)
        {
            List<Verse> results = new List<Verse>();
            
            if (string.IsNullOrWhiteSpace(reference))
            {
                return results;
            }

            // Parse the reference string
            // Expected format: "BookName Chapter:Verse" or "BookName Chapter:Verse-Verse" or "BookName Chapter:Verse,Verse,Verse-Verse"
            string trimmedRef = reference.Trim();
            
            // Split by space to separate book name from chapter:verse
            int lastSpaceIndex = trimmedRef.LastIndexOf(' ');
            if (lastSpaceIndex < 0)
            {
                return results; // Invalid format
            }

            string bookPart = trimmedRef.Substring(0, lastSpaceIndex).Trim();
            string chapterVersePart = trimmedRef.Substring(lastSpaceIndex + 1).Trim();

            // Get book number
            int? bookNumber = Localization.GetBookNumber(bookPart);
            if (!bookNumber.HasValue)
            {
                return results; // Book not found
            }

            // Split chapter and verses
            string[] chapterVerseSplit = chapterVersePart.Split(':');
            if (chapterVerseSplit.Length != 2)
            {
                return results; // Invalid format
            }

            if (!int.TryParse(chapterVerseSplit[0], out int chapterNumber))
            {
                return results; // Invalid chapter number
            }

            string versePart = chapterVerseSplit[1];
            
            // Parse verse specifications (can be single, range, or comma-separated)
            string[] verseSpecs = versePart.Split(',');
            
            foreach (string verseSpec in verseSpecs)
            {
                string trimmedSpec = verseSpec.Trim();
                
                if (trimmedSpec.Contains('-'))
                {
                    // Range: "1-5"
                    string[] rangeParts = trimmedSpec.Split('-');
                    if (rangeParts.Length == 2 &&
                        int.TryParse(rangeParts[0].Trim(), out int startVerse) &&
                        int.TryParse(rangeParts[1].Trim(), out int endVerse))
                    {
                        for (int v = startVerse; v <= endVerse; v++)
                        {
                            Verse? verse = GetVerse(bookNumber.Value, chapterNumber, v);
                            if (verse != null)
                            {
                                results.Add(verse);
                            }
                        }
                    }
                }
                else
                {
                    // Single verse
                    if (int.TryParse(trimmedSpec, out int verseNumber))
                    {
                        Verse? verse = GetVerse(bookNumber.Value, chapterNumber, verseNumber);
                        if (verse != null)
                        {
                            results.Add(verse);
                        }
                    }
                }
            }

            return results;
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
                writer.Write((byte)2); // Version number (incremented to 2 for embedded localization)
                
                // Write Translation and Status
                writer.Write(Translation ?? string.Empty);
                writer.Write(Status ?? string.Empty);
                
                // Write embedded localization data
                string localizationData = Localization.SerializeToString();
                writer.Write(localizationData);
                
                // Write number of testaments
                writer.Write(Testaments.Count);
                
                foreach (TestamentData testament in Testaments)
                {
                    writer.Write((int)testament.Testament);
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