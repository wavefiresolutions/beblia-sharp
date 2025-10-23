﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Beblia.Sharp
{
    /// <summary>
    /// Provides localization mappings between book names and numbers.
    /// </summary>
    public class Localization
    {
        /// <summary>
        /// Maps book names (case-insensitive) to their corresponding book numbers.
        /// </summary>
        private Dictionary<string, int> _bookNames = new Dictionary<string, int>();

        /// <summary>
        /// Reverse lookup dictionary for mapping book numbers to names.
        /// </summary>
        private Dictionary<int, string> _bookNumbers = new Dictionary<int, string>();

        /// <summary>
        /// Maps book numbers to list of abbreviated names (first one is primary).
        /// </summary>
        private Dictionary<int, List<string>> _bookAbbreviations = new Dictionary<int, List<string>>();

        public Localization()
        {
            LoadDefaultLocalization();
        }

        /// <summary>
        /// Normalizes an abbreviation by removing periods and converting to lowercase for matching.
        /// </summary>
        private string NormalizeAbbreviation(string abbreviation)
        {
            return abbreviation.Replace(".", "").ToLowerInvariant();
        }

        /// <summary>
        /// Adds a book with its abbreviations to the dictionaries.
        /// </summary>
        private void AddBook(int number, string fullName, params string[] abbreviations)
        {
            // Add full name
            _bookNumbers[number] = fullName;
            _bookNames[fullName] = number;
            
            // Store abbreviations
            _bookAbbreviations[number] = new List<string>(abbreviations);
            
            // Add all abbreviations (with and without periods) to bookNames
            foreach (var abbrev in abbreviations)
            {
                // Add the abbreviation as-is
                _bookNames[abbrev] = number;
                
                // Also add normalized version (without periods)
                string normalized = NormalizeAbbreviation(abbrev);
                if (normalized != abbrev.ToLowerInvariant())
                {
                    _bookNames[normalized] = number;
                }
            }
        }

        /// <summary>
        /// Loads the default localization data.
        /// </summary>
        private void LoadDefaultLocalization()
        {
            _bookNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _bookNumbers = new Dictionary<int, string>();
            _bookAbbreviations = new Dictionary<int, List<string>>();

            // Old Testament
            AddBook(1, "Genesis", "Gen.", "Ge.", "Gn.");
            AddBook(2, "Exodus", "Ex.", "Exod.", "Exo.");
            AddBook(3, "Leviticus", "Lev.", "Le.", "Lv.");
            AddBook(4, "Numbers", "Num.", "Nu.", "Nm.", "Nb.");
            AddBook(5, "Deuteronomy", "Deut.", "De.", "Dt.");
            AddBook(6, "Joshua", "Josh.", "Jos.", "Jsh.");
            AddBook(7, "Judges", "Judg.", "Jdg.", "Jg.", "Jdgs.");
            AddBook(8, "Ruth", "Ruth", "Rth.", "Ru.");
            AddBook(9, "1 Samuel", "1 Sam.", "1 Sm.", "1 Sa.", "I Sam.", "1st Samuel");
            AddBook(10, "2 Samuel", "2 Sam.", "2 Sm.", "2 Sa.", "II Sam.", "2nd Samuel");
            AddBook(11, "1 Kings", "1 Kings", "1 Kgs", "1 Ki", "I Kgs", "1st Kings");
            AddBook(12, "2 Kings", "2 Kings", "2 Kgs.", "2 Ki.", "II Kgs.", "2nd Kings");
            AddBook(13, "1 Chronicles", "1 Chron.", "1 Chr.", "1 Ch.", "I Chron.", "1st Chronicles");
            AddBook(14, "2 Chronicles", "2 Chron.", "2 Chr.", "2 Ch.", "II Chron.", "2nd Chronicles");
            AddBook(15, "Ezra", "Ezra", "Ezr.", "Ez.");
            AddBook(16, "Nehemiah", "Neh.", "Ne.");
            AddBook(17, "Esther", "Est.", "Esth.", "Es.");
            AddBook(18, "Job", "Job", "Jb.");
            AddBook(19, "Psalms", "Ps.", "Pslm.", "Psa.", "Psm.", "Pss.");
            AddBook(20, "Proverbs", "Prov.", "Pro.", "Prv.", "Pr.");
            AddBook(21, "Ecclesiastes", "Eccles.", "Eccle.", "Ecc.", "Ec.", "Qoh.");
            AddBook(22, "Song of Solomon", "Song", "SOS.", "Cant.");
            AddBook(23, "Isaiah", "Isa.", "Is.");
            AddBook(24, "Jeremiah", "Jer.", "Je.", "Jr.");
            AddBook(25, "Lamentations", "Lam.", "La.");
            AddBook(26, "Ezekiel", "Ezek.", "Eze.", "Ezk.");
            AddBook(27, "Daniel", "Dan.", "Da.", "Dn.");
            AddBook(28, "Hosea", "Hos.", "Ho.");
            AddBook(29, "Joel", "Joel", "Jl.");
            AddBook(30, "Amos", "Amos", "Am.");
            AddBook(31, "Obadiah", "Obad.", "Ob.");
            AddBook(32, "Jonah", "Jonah", "Jnh.", "Jon.");
            AddBook(33, "Micah", "Mic.", "Mc.");
            AddBook(34, "Nahum", "Nah.", "Na.");
            AddBook(35, "Habakkuk", "Hab.", "Hb.");
            AddBook(36, "Zephaniah", "Zeph.", "Zep.", "Zp.");
            AddBook(37, "Haggai", "Hag.", "Hg.");
            AddBook(38, "Zechariah", "Zech.", "Zec.", "Zc.");
            AddBook(39, "Malachi", "Mal.", "Ml.");

            // New Testament
            AddBook(40, "Matthew", "Matt.", "Mt.");
            AddBook(41, "Mark", "Mark", "Mrk", "Mk", "Mr.");
            AddBook(42, "Luke", "Luke", "Luk", "Lk");
            AddBook(43, "John", "John", "Joh", "Jhn", "Jn");
            AddBook(44, "Acts", "Acts", "Act", "Ac");
            AddBook(45, "Romans", "Rom.", "Ro.", "Rm.");
            AddBook(46, "1 Corinthians", "1 Cor.", "1 Co.", "I Cor.", "1st Corinthians");
            AddBook(47, "2 Corinthians", "2 Cor.", "2 Co.", "II Cor.", "2nd Corinthians");
            AddBook(48, "Galatians", "Gal.", "Ga.");
            AddBook(49, "Ephesians", "Eph.", "Ephes.");
            AddBook(50, "Philippians", "Phil.", "Php.", "Pp.");
            AddBook(51, "Colossians", "Col.", "Co.");
            AddBook(52, "1 Thessalonians", "1 Thess.", "1 Thes.", "I Thess.", "1st Thessalonians");
            AddBook(53, "2 Thessalonians", "2 Thess.", "2 Thes.", "II Thess.", "2nd Thessalonians");
            AddBook(54, "1 Timothy", "1 Tim.", "1 Ti.", "I Tim.", "1st Timothy");
            AddBook(55, "2 Timothy", "2 Tim.", "2 Ti.", "II Tim.", "2nd Timothy");
            AddBook(56, "Titus", "Titus", "Tit", "Ti");
            AddBook(57, "Philemon", "Philem.", "Phm.", "Pm.");
            AddBook(58, "Hebrews", "Heb.");
            AddBook(59, "James", "James", "Jas", "Jm");
            AddBook(60, "1 Peter", "1 Pet.", "1 Pe.", "I Pet.", "1st Peter");
            AddBook(61, "2 Peter", "2 Pet.", "2 Pe.", "II Pet.", "2nd Peter");
            AddBook(62, "1 John", "1 John", "1 Jhn.", "I John", "1st John");
            AddBook(63, "2 John", "2 John", "2 Jhn.", "II John", "2nd John");
            AddBook(64, "3 John", "3 John", "3 Jhn.", "III John", "3rd John");
            AddBook(65, "Jude", "Jude", "Jud.", "Jd.");
            AddBook(66, "Revelation", "Rev.", "Re.");
        }

        /// <summary>
        /// Parses a single line from the localization data to extract book number, name, and abbreviations.
        /// </summary>
        /// <param name="trimmedLine">The trimmed line to parse.</param>
        /// <param name="bookNumber">Output: the parsed book number.</param>
        /// <param name="bookFullName">Output: the parsed book name.</param>
        /// <param name="bookAbbreviations">Output: the parsed abbreviations.</param>
        /// <returns>True if parsing succeeded, false otherwise.</returns>
        private bool ParseLocalizationLine(string trimmedLine, out int bookNumber, out string bookFullName, out string[] bookAbbreviations)
        {
            bookNumber = 0;
            bookFullName = string.Empty;
            bookAbbreviations = Array.Empty<string>();

            // Parse format: "number fullname abbreviation1, abbreviation2, ..."
            // First, extract the book number
            int spaceIndex = trimmedLine.IndexOf(' ');
            if (spaceIndex < 0)
            {
                return false; // Invalid format
            }
            
            string bookNumberStr = trimmedLine.Substring(0, spaceIndex);
            if (!int.TryParse(bookNumberStr, out bookNumber))
            {
                return false; // Invalid book number
            }

            // Remaining line contains: "fullname abbreviation1, abbreviation2, ..."
            string remainingLine = trimmedLine.Substring(spaceIndex + 1).TrimStart();
            
            // Strategy: Split by commas to get all parts.
            // The book name is everything before the first abbreviation.
            // The first part (before the first comma) contains both the book name and first abbreviation.
            string[] commaSplit = remainingLine.Split(',');
            
            if (commaSplit.Length == 0)
            {
                return false; // Invalid format
            }
            
            // The first comma-delimited part contains: "bookname firstabbrev"
            string firstPart = commaSplit[0].Trim();
            
            // We need to split firstPart into book name and first abbreviation.
            string[] words = firstPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (words.Length == 0)
            {
                // No words found - invalid format
                return false;
            }
            else if (words.Length == 1)
            {
                // Single word - use it as both name and abbreviation
                bookFullName = words[0];
                bookAbbreviations = commaSplit.Select(a => a.Trim()).Where(a => !string.IsNullOrEmpty(a)).ToArray();
            }
            else
            {
                // Multiple words - need to find the split point
                int bestSplitIndex = words.Length - 1; // Default: last word is the abbreviation
                
                // Look for indicators that suggest where the abbreviation starts:
                // 1. If a word contains a period, it's likely an abbreviation
                // 2. We also need to check if the word before the period is a number (part of the abbreviation)
                bool foundPeriod = false;
                for (int i = 1; i < words.Length; i++)
                {
                    string potentialAbbrev = words[i];
                    // If this word contains a period and previous words in book name don't, split here
                    if (potentialAbbrev.Contains("."))
                    {
                        bool previousHasPeriod = false;
                        for (int j = 0; j < i; j++)
                        {
                            if (words[j].Contains("."))
                            {
                                previousHasPeriod = true;
                                break;
                            }
                        }
                        if (!previousHasPeriod)
                        {
                            bestSplitIndex = i;
                            foundPeriod = true;
                            
                            // Check if the word before this period word is a number or roman numeral
                            // If so, it's part of the abbreviation, so move split point back
                            if (i > 0)
                            {
                                string prevWord = words[i - 1];
                                // Check if previous word is a number (1, 2, 3) or roman numeral (I, II, III)
                                bool isNumber = int.TryParse(prevWord, out _);
                                bool isRomanNumeral = prevWord == "I" || prevWord == "II" || prevWord == "III" || 
                                                     prevWord == "IV" || prevWord == "V";
                                if (isNumber || isRomanNumeral)
                                {
                                    // The previous word is part of the abbreviation
                                    // But only if it's not also the first word (which would be part of book name)
                                    if (i > 1)
                                    {
                                        bestSplitIndex = i - 1;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                
                // If no period was found, check if the first part equals the entire book name
                // (e.g., "1 Kings 1 Kings" where "1 Kings" is both the name and first abbreviation)
                if (!foundPeriod)
                {
                    // Check if firstPart is a repeat of the first half
                    int halfLength = words.Length / 2;
                    if (words.Length % 2 == 0 && halfLength > 0)
                    {
                        bool isRepeat = true;
                        for (int i = 0; i < halfLength; i++)
                        {
                            if (words[i] != words[halfLength + i])
                            {
                                isRepeat = false;
                                break;
                            }
                        }
                        if (isRepeat)
                        {
                            // First half is repeated, so use first half as book name
                            bestSplitIndex = halfLength;
                        }
                    }
                }
                
                // Build book name from words before split point
                bookFullName = string.Join(" ", words, 0, bestSplitIndex);
                
                // Build abbreviations list
                string firstAbbrev = string.Join(" ", words, bestSplitIndex, words.Length - bestSplitIndex);
                var abbrevList = new List<string> { firstAbbrev };
                for (int i = 1; i < commaSplit.Length; i++)
                {
                    string abbrev = commaSplit[i].Trim();
                    if (!string.IsNullOrEmpty(abbrev))
                    {
                        abbrevList.Add(abbrev);
                    }
                }
                bookAbbreviations = abbrevList.ToArray();
            }

            return true;
        }

        /// <summary>
        /// Loads localization from a text file.
        /// Format: number fullname abbreviation1, abbreviation2, abbreviation3, ...
        /// (e.g., "1 Genesis Gen., Ge., Gn.")
        /// </summary>
        /// <param name="filePath">Path to the localization file.</param>
        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Localization file not found.", filePath);
            }

            _bookNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _bookNumbers = new Dictionary<int, string>();
            _bookAbbreviations = new Dictionary<int, List<string>>();

            foreach (string line in File.ReadAllLines(filePath))
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                {
                    continue; // Skip empty lines and comments
                }

                if (ParseLocalizationLine(trimmedLine, out int bookNumber, out string bookFullName, out string[] bookAbbreviations))
                {
                    AddBook(bookNumber, bookFullName, bookAbbreviations);
                }
            }
        }

        /// <summary>
        /// Loads localization from a string (used for embedded data).
        /// Format: number fullname abbreviation1, abbreviation2, abbreviation3, ...
        /// </summary>
        /// <param name="localizationData">The localization data as a string.</param>
        public void LoadFromString(string localizationData)
        {
            _bookNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _bookNumbers = new Dictionary<int, string>();
            _bookAbbreviations = new Dictionary<int, List<string>>();

            foreach (string line in localizationData.Split('\n'))
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                {
                    continue; // Skip empty lines and comments
                }

                if (ParseLocalizationLine(trimmedLine, out int bookNumber, out string bookFullName, out string[] bookAbbreviations))
                {
                    AddBook(bookNumber, bookFullName, bookAbbreviations);
                }
            }
        }

        /// <summary>
        /// Gets the book number for a given book name or abbreviation (case-insensitive).
        /// </summary>
        /// <param name="bookName">The name or abbreviation of the book.</param>
        /// <returns>The book number, or null if not found.</returns>
        public int? GetBookNumber(string bookName)
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
        public string? GetBookName(int bookNumber)
        {
            if (_bookNumbers.TryGetValue(bookNumber, out string? name))
            {
                return name;
            }
            return null;
        }

        /// <summary>
        /// Gets the abbreviated book name for a given book number.
        /// Returns the primary (first) abbreviation.
        /// </summary>
        /// <param name="bookNumber">The book number.</param>
        /// <returns>The abbreviated book name, or null if not found.</returns>
        public string? GetBookAbbreviation(int bookNumber)
        {
            if (_bookAbbreviations.TryGetValue(bookNumber, out List<string>? abbreviations) && abbreviations.Count > 0)
            {
                return abbreviations[0];
            }
            return null;
        }

        /// <summary>
        /// Gets all abbreviated book names for a given book number.
        /// </summary>
        /// <param name="bookNumber">The book number.</param>
        /// <returns>A list of abbreviations, or an empty list if not found.</returns>
        public List<string> GetBookAbbreviations(int bookNumber)
        {
            if (_bookAbbreviations.TryGetValue(bookNumber, out List<string>? abbreviations))
            {
                return new List<string>(abbreviations);
            }
            return new List<string>();
        }

        /// <summary>
        /// Serializes the current localization data to a string format.
        /// Format: number fullname abbreviation1, abbreviation2, ...
        /// </summary>
        /// <returns>Localization data as a string.</returns>
        public string SerializeToString()
        {
            var lines = new List<string>();
            lines.Add("# Beblia Localization File");
            lines.Add("# Format: number fullname abbreviation1, abbreviation2, abbreviation3, ...");
            lines.Add("");

            foreach (var bookNumber in _bookNumbers.Keys.OrderBy(k => k))
            {
                string fullName = _bookNumbers[bookNumber];
                List<string> abbreviations = GetBookAbbreviations(bookNumber);
                string abbreviationsStr = string.Join(", ", abbreviations);
                lines.Add($"{bookNumber} {fullName} {abbreviationsStr}");
            }

            return string.Join("\n", lines);
        }
    }
}