using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SeenAPI.Utilits
{
    public static class clsUtile
    {
        private static bool CreateFolderIfNotExists(string folderPath)
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

        public static byte[]? SetImageData(string folderPath, string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return null;

            if (CreateFolderIfNotExists(folderPath))
            {
                string fullPath = Path.Combine(folderPath, imagePath);
                if (File.Exists(fullPath))
                {
                    return File.ReadAllBytes(fullPath);
                }
            }
            return null;
        }

        public static void DeleteFile(string folderPath, string? filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            if (CreateFolderIfNotExists(folderPath))
            {
                string fullPath = Path.Combine(folderPath, filePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }
    }
}