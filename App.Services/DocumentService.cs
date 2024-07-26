
using Microsoft.AspNetCore.Http;

namespace App.Services
{
    public class DocumentService
    {
        public static async Task<string> SaveDocumentToFolder(IFormFile file, string folderName, string docName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Documents");
                folder = Path.Combine(folder, folderName);

                if (!(Directory.Exists(folder)))
                {
                    Directory.CreateDirectory(folder);
                }
                // Check if the file is not null
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null.");
                }

                // Validate the file format
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (fileExtension != ".pdf" && fileExtension != ".txt")
                {
                    throw new ArgumentException("Invalid file format. Only PDF or TXT files are allowed.");
                }

                string uniqueFileName = docName + fileExtension;
                string filePath = Path.Combine(folder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // File saved successfully
                return uniqueFileName;
            }
            catch
            {
                throw new FileNotFoundException("Error processing.");
            }
        }

        public static string ReadDocumentFromFolder(string docName, string folderName)
        {
            var roota = AppDomain.CurrentDomain;
            var root = roota.BaseDirectory;
            string folder = Path.Combine(root, "Documents");
            folder = Path.Combine(folder, folderName);

            string filePath = Path.Combine(folder, docName);

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.");
            }

            // Read the file contents
            string fileContents;
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fileStream))
            {
                fileContents = streamReader.ReadToEnd();
            }

            // Return the file contents
            return fileContents;
        }
        public static string GetDocumentPath(string uniqueFileName, string folderName)
        {
            string folder = Path.Combine("/docs", folderName);

            string filePath = Path.Combine(folder, uniqueFileName);

            return filePath;
        }

    }

}