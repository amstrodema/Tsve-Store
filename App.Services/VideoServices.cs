using Microsoft.AspNetCore.Http;

namespace App.Services
{
    public class VideoServices
    {
        public static async Task<string> SaveVideoFile(IFormFile videoFile, string folderName, string videoName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Videos");
                folder = Path.Combine(folder, folderName);
                string uniqueFileName = videoName + ".mp4";
                string filePath = Path.Combine(folder, uniqueFileName);

                if (!(Directory.Exists(folder)))
                {
                    Directory.CreateDirectory(folder);
                }

                // Validate the file extension
                var fileExtension = Path.GetExtension(videoFile.FileName);
                if (fileExtension.ToLower() != ".mp4")
                {
                    FileService.WriteToFile("\n\nInvalid file extension", "ErrorLogs");
                    // Invalid file extension
                    throw new Exception();
                }

                // Save the video file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }

                return uniqueFileName;
            }
            catch (Exception e)
            {
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
                throw;
            }

        }
        public static string GetVideoFromFolder(string uniqueFileName, string folderName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Videos");
                folder = Path.Combine(folder, folderName);
                string filePath = Path.Combine(folder, uniqueFileName);

                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Read the video file in chunks and write to the memory stream
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            memoryStream.Write(buffer, 0, bytesRead);
                        }

                        // Convert the memory stream to a base64 string
                        string base64String = Convert.ToBase64String(memoryStream.ToArray());

                        return "data:video/mp4;base64," + base64String;
                    }
                }
            }
            catch (Exception e)
            {
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
                throw;
            }
        }
        public static string GetVideoPath(string uniqueFileName, string folderName)
        {
            string folder = Path.Combine("/vids", folderName);

            string filePath = Path.Combine(folder, uniqueFileName);

            return filePath;
        }
    }

}