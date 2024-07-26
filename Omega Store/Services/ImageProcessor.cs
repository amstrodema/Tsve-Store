namespace Omega_Store.Services
{
    public class ImageProcessor
    {

        public static async Task<byte[]> GetByte(IFormFile formFile)
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
    }
}
