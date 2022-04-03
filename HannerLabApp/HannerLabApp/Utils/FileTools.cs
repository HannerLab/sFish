using System;
using System.IO;
using System.IO.Compression;

namespace HannerLabApp.Utils
{
    public static class FileTools
    {
        public static bool ZipFolderContents(string folderToZip, string destinationZipFullPath)
        {
            try
            {
                // Delete existing zip file if exists
                if (File.Exists(destinationZipFullPath))
                    File.Delete(destinationZipFullPath);

                ZipFile.CreateFromDirectory(sourceDirectoryName: folderToZip, destinationArchiveFileName: destinationZipFullPath, CompressionLevel.Optimal, false);

                return File.Exists(destinationZipFullPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }

        public static bool QuickZip(string[] filesToZip, string destinationZipFullPath)
        {
            try
            {
                // Delete existing zip file if exists
                if (File.Exists(destinationZipFullPath))
                    File.Delete(destinationZipFullPath);

                using (ZipArchive zip = ZipFile.Open(destinationZipFullPath, ZipArchiveMode.Create))
                {
                    foreach (var file in filesToZip)
                    {
                        zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
                    }
                }

                return File.Exists(destinationZipFullPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }
	}
}
