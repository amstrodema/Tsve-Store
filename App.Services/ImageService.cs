using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp;
using Store.Model.ViewModel;
using System.Text.Json;
using SixLabors.ImageSharp.Processing;

namespace App.Services
{
    public class ImageService
    {
        private static async Task ResizeImage(IFormFile formFile, int width, int height, string imageID, string folderName)
        {
            byte[] bytes = await GetByte(formFile);
            // Load the image from the byte array using ImageSharp
            using (Image image = Image.Load(bytes))
            {
                // Resize the image while retaining its aspect ratio and quality
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max,
                    Compand = false,
                    Sampler = KnownResamplers.Lanczos3,
                }));


                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Images");
                folder = Path.Combine(folder, "Small");
                folder = Path.Combine(folder, folderName);
                string uniqueFileName = imageID + ".png";
                string filePath = Path.Combine(folder, uniqueFileName);

                if (!(Directory.Exists(folder)))
                {
                    Directory.CreateDirectory(folder);
                }

                // Convert the image back to a base64 string
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, new PngEncoder());
                    byte[] imageBytes = ms.ToArray();

                    try
                    {

                        MemoryStream ms2 = new MemoryStream(imageBytes);

                        try
                        {
                            using (Stream stream = new FileStream(filePath, FileMode.Create))
                            {
                                ms2.CopyTo(stream);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    catch (Exception e)
                    {
                        FileService.WriteToFile("\n\n" + e, "ErrorLogs");
                        throw;
                    }

                }
            }
        }

        public static async Task<string> SaveImageInFolder(IFormFile imageFile, string imageID, string folderName, bool makeSmaller = true)
        {
            var roota = AppDomain.CurrentDomain;
            var root = roota.BaseDirectory;
            string folder = Path.Combine(root, "Images");
            folder = Path.Combine(folder, "Large");
            folder = Path.Combine(folder, folderName);
            string uniqueFileName = imageID + ".png";
            string filePath = Path.Combine(folder, uniqueFileName);

            if (!(Directory.Exists(folder)))
            {
                Directory.CreateDirectory(folder);
            }

            if (makeSmaller)
            {
                try
                {
                    await ResizeImage(imageFile, 250, 250, imageID, folderName);
                }
                catch (Exception e)
                {
                    FileService.WriteToFile("\n\n" + e, "ErrorLogs");
                    throw;
                }
            }


            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return uniqueFileName;
            //return filePath;
        }
        public static string GetLargeImagePath(string uniqueFileName, string folderName)
        {
            try
            {
                //var roota = AppDomain.CurrentDomain;
                //var root = roota.BaseDirectory;
                //string folder = Path.Combine(root, "Images");
                string folder = Path.Combine("../trig", "Large");
                folder = Path.Combine(folder, folderName);

                string filePath = Path.Combine(folder, uniqueFileName);
                return filePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string GetSmallImagePath(string uniqueFileName, string folderName)
        {
            //var roota = AppDomain.CurrentDomain;
            //var root = roota.BaseDirectory;
            //string folder = Path.Combine(root, "Images");
            string folder = Path.Combine("../trig", "Small");
            folder = Path.Combine(folder, folderName);

            string filePath = Path.Combine(folder, uniqueFileName);

            return filePath;
        }
        public static string GetImageFromFolder(string uniqueFileName, string folderName)
        {

            try
            {

                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Images");
                folder = Path.Combine(folder, "Large");
                folder = Path.Combine(folder, folderName);

                string filePath = Path.Combine(folder, uniqueFileName);

                byte[] imageArray = File.ReadAllBytes(filePath);
                string val = "data:image/png;base64," + Convert.ToBase64String(imageArray);

                return val;
            }
            catch (Exception e)
            {
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
                return string.Empty;
            }
        }

        public static string GetSmallImageFromFolder(string uniqueFileName, string folderName)
        {


            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Images");
                folder = Path.Combine(folder, "Small");
                folder = Path.Combine(folder, folderName);

                string filePath = Path.Combine(folder, uniqueFileName);

                byte[] imageArray = File.ReadAllBytes(filePath);
                string val = "data:image/png;base64," + Convert.ToBase64String(imageArray);

                return val;
            }
            catch (Exception e)
            {
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
                return "";
            }
        }

        public static void DeleteImage(string imageID, string folderName)
        {
            var roota = AppDomain.CurrentDomain;
            var root = roota.BaseDirectory;
            string folder = Path.Combine(root, "Images");
            folder = Path.Combine(folder, "Large");
            folder = Path.Combine(folder, folderName);
            string uniqueFileName = imageID;
            string filePath = Path.Combine(folder, uniqueFileName);

            // Specify the file path
            //string filePath = @"C:\path\to\your\file.txt";

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Delete the file
                    File.Delete(filePath);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static async Task<byte[]> GetByte(IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                //if (stream.Length < 2097152)
                //{
                //}

                return stream.ToArray();
            }

        }

        public static string GetThumbnail(string type, string fileName, string folder)
        {
            if (type == "Img") return GetSmallImagePath(fileName, folder);
            else if (type == "Vid") return "Vid";
            else
            {
                var val = fileName.Split('.');
                if (val[val.Count() - 1] == "pdf")
                {
                    return "../img/icn-pdf.png";
                }
                return "../img/icn-txt2.png";
            }
        }
    }

}