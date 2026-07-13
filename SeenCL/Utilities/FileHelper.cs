using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SeenCL.Utilities
{
    public static class FileHelper
    {
        public static bool CreateFolderIfNotExists(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<string?> SaveFileAsync(IFormFile? file, string folderPath)
        {
            if (file == null) return null;

            try
            {
                if (!CreateFolderIfNotExists(folderPath))
                    return null;

                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                string fileName = $"{Guid.NewGuid()}{fileExtension}";
                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Reads binary image data from a physical path.
        /// </summary>
        /// <param name="folderPath">The base directory (e.g. from appsettings).</param>
        /// <param name="imagePath">The relative path stored in the database.</param>
        /// <returns>Byte array of the image data.</returns>
        public static byte[]? GetImageData(string folderPath, string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return null;

            try
            {
                string fullPath = Path.Combine(folderPath, imagePath);
                if (File.Exists(fullPath))
                {
                    return File.ReadAllBytes(fullPath);
                }
            }
            catch (Exception)
            {
                // Silently fail if image cannot be loaded
            }
            return null;
        }

        public static void DeleteFile(string folderPath, string? filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            try
            {
                string fullPath = Path.Combine(folderPath, filePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception)
            {
                // Silently fail
            }
        }
    }
}
