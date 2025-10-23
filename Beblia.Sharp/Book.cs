using System.Collections.Generic;

namespace Beblia.Sharp
{
    /// <summary>
    /// Represents a single book, containing a list of chapters.
    /// </summary>
    public class Book
    {
        public int Number { get; set; }
        public List<Chapter> Chapters { get; set; }

        public Book()
        {
            Chapters = new List<Chapter>();
        }
    }
}