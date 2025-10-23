using System;
using System.IO;
using System.Xml.Linq;

namespace Beblia.Sharp
{
    /// <summary>
    /// Static class to load a Bible from an XML source.
    /// </summary>
    public static class BibleParser
    {
        /// <summary>
        /// Loads a Bible from an XML file path.
        /// </summary>
        /// <param name="filePath">The path to the .xml file.</param>
        /// <returns>A populated Bible object.</returns>
        public static Bible Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Bible XML file not found.", filePath);
            }

            using (FileStream stream = File.OpenRead(filePath))
            {
                return Load(stream);
            }
        }

        /// <summary>
        /// Loads a Bible from an XML string.
        /// </summary>
        /// <param name="xmlContent">The string containing the Bible XML.</param>
        /// <returns>A populated Bible object.</returns>
        public static Bible LoadXml(string xmlContent)
        {
            using (StringReader reader = new StringReader(xmlContent))
            {
                XDocument doc = XDocument.Load(reader);
                return Parse(doc);
            }
        }

        /// <summary>
        /// Loads a Bible from a Stream (e.g., FileStream, MemoryStream).
        /// </summary>
        /// <param name="stream">The stream containing the Bible XML.</param>
        /// <returns>A populated Bible object.</returns>
        public static Bible Load(Stream stream)
        {
            try
            {
                XDocument doc = XDocument.Load(stream);
                return Parse(doc);
            }
            catch (System.Xml.XmlException ex)
            {
                throw new ArgumentException("Failed to parse XML stream. See inner exception.", ex);
            }
        }

        /// <summary>
        /// Private helper to parse the loaded XDocument into a Bible object.
        /// </summary>
        private static Bible Parse(XDocument doc)
        {
            XElement? root = doc.Root;
            if (root == null || root.Name.LocalName != "bible")
            {
                throw new ArgumentException("XML root element is not <bible> or is missing.");
            }

            Bible bible = new Bible
            {
                // Use null-conditional operator (?) to safely get attribute values
                Translation = root.Attribute("translation")?.Value,
                Status = root.Attribute("status")?.Value
            };

            // Loop through <testament> elements
            foreach (XElement testamentNode in root.Elements("testament"))
            {
                Testament testament = new Testament
                {
                    Name = testamentNode.Attribute("name")?.Value
                };

                // Loop through <book> elements
                foreach (XElement bookNode in testamentNode.Elements("book"))
                {
                    int.TryParse(bookNode.Attribute("number")?.Value, out int bookNum);
                    Book book = new Book { Number = bookNum };

                    // Loop through <chapter> elements
                    foreach (XElement chapterNode in bookNode.Elements("chapter"))
                    {
                        int.TryParse(chapterNode.Attribute("number")?.Value, out int chapNum);
                        Chapter chapter = new Chapter { Number = chapNum };

                        // Loop through <verse> elements
                        foreach (XElement verseNode in chapterNode.Elements("verse"))
                        {
                            int.TryParse(verseNode.Attribute("number")?.Value, out int verseNum);
                            Verse verse = new Verse
                            {
                                Number = verseNum,
                                Text = verseNode.Value // .Value gets the inner text
                            };
                            chapter.Verses.Add(verse);
                        }
                        book.Chapters.Add(chapter);
                    }
                    testament.Books.Add(book);
                }
                bible.Testaments.Add(testament);
            }
            
            return bible;
        }
    }
}