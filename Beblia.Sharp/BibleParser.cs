using System;
using System.IO;
using System.Xml.Linq;

namespace Beblia.Sharp
{
    /// <summary>
    /// Static class to load a Bible from an XML or binary source.
    /// </summary>
    public static class BibleParser
    {
        /// <summary>
        /// Loads a Bible from an XML or binary file path.
        /// Automatically detects the file format based on content.
        /// </summary>
        /// <param name="filePath">The path to the .xml or .beblia file.</param>
        /// <returns>A populated Bible object.</returns>
        public static Bible Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Bible file not found.", filePath);
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
        /// Automatically detects the file format (XML or binary).
        /// </summary>
        /// <param name="stream">The stream containing the Bible data.</param>
        /// <returns>A populated Bible object.</returns>
        public static Bible Load(Stream stream)
        {
            // Peek at the first few bytes to determine format
            byte[] header = new byte[8];
            long originalPosition = stream.Position;
            int bytesRead = stream.Read(header, 0, 8);
            stream.Position = originalPosition; // Reset position
            
            // Check if it's a binary .beblia file
            // Format: length-prefix (1 byte: 0x06) + "BEBLIA" (6 bytes) + version (1 byte)
            if (bytesRead >= 8 && 
                header[0] == 0x06 && // Length of "BEBLIA"
                header[1] == (byte)'B' && 
                header[2] == (byte)'E' && 
                header[3] == (byte)'B' && 
                header[4] == (byte)'L' && 
                header[5] == (byte)'I' && 
                header[6] == (byte)'A' &&
                (header[7] == 0x01 || header[7] == 0x02)) // Version 1 or 2
            {
                return LoadBinary(stream);
            }
            
            // Otherwise, try to parse as XML
            try
            {
                XDocument doc = XDocument.Load(stream);
                return Parse(doc);
            }
            catch (System.Xml.XmlException ex)
            {
                throw new ArgumentException("Failed to parse stream as XML or binary. See inner exception.", ex);
            }
        }

        /// <summary>
        /// Loads a Bible from a binary stream (.beblia format).
        /// </summary>
        /// <param name="stream">The stream containing the binary Bible data.</param>
        /// <returns>A populated Bible object.</returns>
        private static Bible LoadBinary(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream, System.Text.Encoding.UTF8, true))
            {
                // Read and verify magic number
                string magic = reader.ReadString();
                if (magic != "BEBLIA")
                {
                    throw new ArgumentException("Invalid binary format: Missing BEBLIA header.");
                }
                
                byte version = reader.ReadByte();
                if (version != 1 && version != 2)
                {
                    throw new ArgumentException($"Unsupported binary format version: {version}");
                }
                
                Bible bible = new Bible
                {
                    Translation = reader.ReadString(),
                    Status = reader.ReadString()
                };
                
                // Version 2 includes embedded localization
                if (version == 2)
                {
                    string localizationData = reader.ReadString();
                    if (!string.IsNullOrEmpty(localizationData))
                    {
                        Localization.LoadFromString(localizationData);
                    }
                }
                
                int testamentCount = reader.ReadInt32();
                for (int t = 0; t < testamentCount; t++)
                {
                    TestamentData testament = new TestamentData
                    {
                        Testament = (Testament)reader.ReadInt32()
                    };
                    
                    int bookCount = reader.ReadInt32();
                    for (int b = 0; b < bookCount; b++)
                    {
                        Book book = new Book
                        {
                            Number = reader.ReadInt32()
                        };
                        
                        int chapterCount = reader.ReadInt32();
                        for (int c = 0; c < chapterCount; c++)
                        {
                            Chapter chapter = new Chapter
                            {
                                Number = reader.ReadInt32()
                            };
                            
                            int verseCount = reader.ReadInt32();
                            for (int v = 0; v < verseCount; v++)
                            {
                                Verse verse = new Verse
                                {
                                    Number = reader.ReadInt32(),
                                    Text = reader.ReadString()
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
                string? testamentName = testamentNode.Attribute("name")?.Value;
                Testament testamentType = testamentName?.ToLowerInvariant() == "new" ? Testament.New : Testament.Old;
                
                TestamentData testament = new TestamentData
                {
                    Testament = testamentType
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