using Azure.Storage.Blobs;
namespace NzWalks.Service.Blob;

public interface IBlobService
{
    Task<string> UploadFileAsync(IFormFile file, string fileName);
     Task<bool> DeleteFileAsync(string fileName);
}
