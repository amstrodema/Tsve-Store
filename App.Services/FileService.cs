using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace App.Services
{
    public class FileService
    {


        public static void WriteToFile(string data, string fileName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Files");
                string uniqueFileName = fileName + ".txt";
                string filePath = Path.Combine(folder, uniqueFileName);

                List<string> readData = new List<string>();
                if (!(Directory.Exists(folder)))
                {
                    Directory.CreateDirectory(folder);
                }
                else
                {
                    try
                    {
                        readData = File.ReadAllLines(filePath).ToList();
                    }
                    catch (Exception)
                    {
                    }
                }

                int count = 0;

                // Create or overwrite the text file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string item in readData)
                    {
                        // Write each item to a new line in the file
                        if (count > 0)
                        {
                            writer.Write(",");
                        }
                        writer.Write(item);
                        count++;
                    }

                    if (count > 0)
                    {
                        writer.Write(",");
                    }
                    writer.Write(data);
                }
            }
            catch (IOException)
            {
                throw;
            }
        }
        public static string ToBase64(IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    string base64String = Convert.ToBase64String(fileBytes);
                    return base64String;
                }
            }
            catch (IOException)
            {
                throw;
            }
        }
        public static void UpdateFile(string data, string fileName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Files");
                string uniqueFileName = fileName + ".txt";
                string filePath = Path.Combine(folder, uniqueFileName);

                // Create or overwrite the text file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(data.Substring(1, data.Length - 2));
                }
            }
            catch (IOException)
            {
                throw;
            }
        }

        public static string ReadFromFile(string fileName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Files");
                string uniqueFileName = fileName + ".txt";
                string filePath = Path.Combine(folder, uniqueFileName);
                // Read all lines from the text file into an array
                var readData = File.ReadAllLines(filePath);

                return "[" + readData[0] + "]";
            }
            catch (IOException)
            {
                return "";
            }


        }
        public static void DeleteFile(string fileName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Files");
                string uniqueFileName = fileName + ".txt";
                string filePath = Path.Combine(folder, uniqueFileName);

                File.Delete(filePath);
            }
            catch (IOException)
            {
            }


        }
        public static List<T> Shuffle<T>(List<T> list)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                Random rng = new Random();
                int n = list.Count;
                int count = 0;

                stopwatch.Start();
                while (n > 1)
                {
                    if (stopwatch.Elapsed.TotalSeconds > 2)
                    {
                        stopwatch.Stop();
                        break;
                    }
                    n--;
                    int k = rng.Next(n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;

                    count++;
                }
            }
            catch (Exception)
            {
            }

            return list;
        }
    }

}