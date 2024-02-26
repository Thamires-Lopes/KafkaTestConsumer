using KafkaTestConsumer.Models;
using System.IO.Compression;

namespace KafkaTestConsumer.Helpers
{
    public static class ZipFileHelper
    {
        private const string TEMP_ARCHIVE = "tempArchive.txt";
        private const string ARCHIVE_NAME = "usersArchive.txt";
        private const string ZIP_NAME = "usersZip.zip";

        public static void WriteFile(User user)
        {
            var directory = Directory.GetCurrentDirectory();

            var tempArchivePath = Path.Combine(directory, TEMP_ARCHIVE);
            var zipPath = Path.Combine(directory, ZIP_NAME);

            try
            {
                if (File.Exists(zipPath))
                {
                    UpdateZip(user, tempArchivePath, zipPath);
                }
                else
                {
                    CreateZip(user, zipPath);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CreateZip(User user, string zipPath)
        {
            using (var zip = new FileStream(zipPath, FileMode.Create))
            {
                using (var archive = new ZipArchive(zip, ZipArchiveMode.Create))
                {
                    var zipArhiveEntry = archive.CreateEntry(ARCHIVE_NAME);

                    using (var writer = new StreamWriter(zipArhiveEntry.Open()))
                    {
                        writer.WriteLine($"User received - Name: {user.Name} | Id: {user.Id}");
                    }
                }
            }
        }

        private static void UpdateZip(User user, string tempArchivePath, string zipPath)
        {
            using (var zip = new FileStream(zipPath, FileMode.Open))
            {
                using (var archive = new ZipArchive(zip, ZipArchiveMode.Update))
                {
                    var zipArchiveEntry = archive.GetEntry(ARCHIVE_NAME);

                    if (zipArchiveEntry != null)
                    {
                        using (var tempFileStream = new FileStream(tempArchivePath, FileMode.Create))
                        {
                            using (var oldStream = zipArchiveEntry.Open())
                            {
                                oldStream.CopyTo(tempFileStream);
                            }
                        }

                        using (var tempWriter = new StreamWriter(tempArchivePath, true))
                        {
                            tempWriter.WriteLine($"User received - Name: {user.Name} | Id: {user.Id}");
                        }

                        zipArchiveEntry.Delete();
                    }

                    var newZipArchiveEntry = archive.CreateEntry(ARCHIVE_NAME);

                    using (var fileStream = new FileStream(tempArchivePath, FileMode.Open))
                    {
                        using (var writer = newZipArchiveEntry.Open())
                        {
                            fileStream.CopyTo(writer);
                        }
                    }

                    File.Delete(tempArchivePath);
                }
            }
        }
    }
}
