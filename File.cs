using System;
using System.IO;
using System.Linq;

class OfficeFilesSummary
{
    static void Main(string[] args)
    {
        // Step 1: Setup Directory and File Paths
        string directoryPath = @"C:\FileCollection";
        string resultsFilePath = Path.Combine(directoryPath, "results.txt");

        // Ensure the directory exists
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Step 2: Create Helper Function
        bool IsOfficeFile(string extension)
        {
            string[] officeExtensions = { ".xlsx", ".docx", ".pptx" };
            return officeExtensions.Contains(extension.ToLower());
        }

        // Step 3: Initialize Counters and Variables
        int xlsxCount = 0, docxCount = 0, pptxCount = 0;
        long xlsxSize = 0, docxSize = 0, pptxSize = 0;
        int totalCount = 0;
        long totalSize = 0;

        // Step 4: Directory Info Object
        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

        // Step 5: Enumerate Files
        foreach (var file in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
        {
            if (IsOfficeFile(file.Extension))
            {
                totalCount++;
                totalSize += file.Length;

                switch (file.Extension.ToLower())
                {
                    case ".xlsx":
                        xlsxCount++;
                        xlsxSize += file.Length;
                        break;
                    case ".docx":
                        docxCount++;
                        docxSize += file.Length;
                        break;
                    case ".pptx":
                        pptxCount++;
                        pptxSize += file.Length;
                        break;
                }
            }
        }

        // Step 6: Write Results to File
        using (StreamWriter writer = new StreamWriter(resultsFilePath))
        {
            writer.WriteLine("Microsoft Office Files Summary:");
            writer.WriteLine($"Total Count: {totalCount}");
            writer.WriteLine($"Total Size: {FormatSize(totalSize)}\n");

            writer.WriteLine($"Excel (.xlsx) Files: Count = {xlsxCount}, Size = {FormatSize(xlsxSize)}");
            writer.WriteLine($"Word (.docx) Files: Count = {docxCount}, Size = {FormatSize(docxSize)}");
            writer.WriteLine($"PowerPoint (.pptx) Files: Count = {pptxCount}, Size = {FormatSize(pptxSize)}");
        }

        // Optionally display the summary in the console
        Console.WriteLine(File.ReadAllText(resultsFilePath));
    }

    // Helper method to format file size in a readable format
    static string FormatSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double formattedSize = bytes;
        int sizeIndex = 0;

        while (formattedSize >= 1024 && sizeIndex < sizes.Length - 1)
        {
            sizeIndex++;
            formattedSize /= 1024;
        }

        return $"{formattedSize:0.##} {sizes[sizeIndex]}";
    }
}
