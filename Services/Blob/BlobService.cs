using Azure.Storage.Blobs;

using Azure.Storage.Sas;
namespace NzWalks.Service.Blob;

    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobService(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration["AzureBlobStorage:ConnectionString"]);
            _containerName = configuration["AzureBlobStorage:ContainerName"];
        }

    public async Task<string> UploadFileAsync(IFormFile file, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(fileName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        // Generate SAS token (valid for 24 hours for example)
        if (blobClient.CanGenerateSasUri)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerName,
                BlobName = fileName,
                Resource = "b", // b = blob
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(24) // expires after 1 day
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }

        // fallback: return raw URL (not useful unless container public)
        return blobClient.Uri.ToString();
    }
    


        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            var response = await blobClient.DeleteIfExistsAsync();
            return response.Value;
        }

    }


