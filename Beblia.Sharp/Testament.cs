using System.Collections.Generic;

namespace Beblia.Sharp
{
    /// <summary>
    /// Represents a testament (e.g., Old or New), containing a list of books.
    /// </summary>
    public class Testament
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        public Testament()
        {
            Books = new List<Book>();
        }
    }
}