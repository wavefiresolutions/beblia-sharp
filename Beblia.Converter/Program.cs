using Beblia.Sharp;

Console.WriteLine("Beblia XML to Binary Converter");
Console.WriteLine("================================\n");

if (args.Length == 0)
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  Beblia.Converter <input.xml> [output.beblia]");
    Console.WriteLine("  Beblia.Converter <input1.xml> <input2.xml> ... (converts multiple files)");
    Console.WriteLine("\nExamples:");
    Console.WriteLine("  Beblia.Converter EnglishKJV.xml");
    Console.WriteLine("  Beblia.Converter EnglishKJV.xml KJV.beblia");
    Console.WriteLine("  Beblia.Converter Bible1.xml Bible2.xml Bible3.xml");
    return;
}

int successCount = 0;
int errorCount = 0;

foreach (string inputFile in args)
{
    try
    {
        // Determine output file name
        string outputFile;
        if (args.Length == 2 && !inputFile.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
        {
            // If there are exactly 2 args and the second doesn't end with .xml, treat it as the output file
            outputFile = args[1];
        }
        else
        {
            // Otherwise, generate output file name by replacing .xml with .beblia
            outputFile = Path.ChangeExtension(inputFile, ".beblia");
        }

        Console.Write($"Converting {inputFile} to {outputFile}... ");

        // Load the Bible from XML
        Bible bible = BibleParser.Load(inputFile);

        // Save it in binary format
        bible.SaveBinary(outputFile);

        // Get file size info
        long xmlSize = new FileInfo(inputFile).Length;
        long binarySize = new FileInfo(outputFile).Length;
        double ratio = (double)binarySize / xmlSize * 100;

        Console.WriteLine($"Done! (Size: {FormatBytes(xmlSize)} -> {FormatBytes(binarySize)}, {ratio:F1}%)");
        successCount++;

        // If we had exactly 2 args and second was output file, stop after first conversion
        if (args.Length == 2 && !args[1].EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        errorCount++;
    }
}

Console.WriteLine($"\nConversion complete: {successCount} succeeded, {errorCount} failed.");

static string FormatBytes(long bytes)
{
    string[] sizes = { "B", "KB", "MB", "GB" };
    double len = bytes;
    int order = 0;
    while (len >= 1024 && order < sizes.Length - 1)
    {
        order++;
        len = len / 1024;
    }
    return $"{len:0.##} {sizes[order]}";
}
