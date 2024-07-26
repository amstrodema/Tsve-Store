using Microsoft.AspNetCore.Http;

namespace App.Services
{
    using FirebaseAdmin;
    using Google.Cloud.Storage.V1;
    using Google.Apis.Auth.OAuth2;

    public class FirebaseService
    {
        private readonly StorageClient _storageClient;
        private readonly FirebaseApp _firebaseApp;

        public FirebaseService()
        {
            var roota = AppDomain.CurrentDomain;
            var root = roota.BaseDirectory;
            string folder = Path.Combine(root, "wwwroot");
            var credentialPath = Path.Combine(folder, "omega-stores-a7d01-firebase-adminsdk-i3e8x-ef93e32ebb.json");
            try
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "omega-stores-a7d01-firebase-adminsdk-i3e8x-77960c134b.json");
                _firebaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("omega-stores-a7d01-firebase-adminsdk-i3e8x-77960c134b.json")
            });

            }
            catch (Exception)
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
            }
         
            _storageClient = StorageClient.Create();
        }

        public async Task<string> UploadImageAsync(IFormFile file, string fileName)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new Exception("No file uploaded.");

                var bucketName = "omega-stores-a7d01.appspot.com";
                var remoteFileName = fileName+".png"; // or any other logic to generate a unique name

                using (var fileStream = file.OpenReadStream()) // Use file.OpenReadStream() to access the file stream
                {
                    var objectName = remoteFileName;
                    var uploadObject = await _storageClient.UploadObjectAsync(bucketName, objectName, null, fileStream);
                    return uploadObject.MediaLink;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<Stream> DownloadImageAsync(string bucketName, string remoteFileName)
        //{
        //    var objectName = remoteFileName;
        //    var downloadObject = await _storageClient.GetObjectAsync(bucketName, objectName);
        //    return downloadObject.Content;
        //}
    }


}