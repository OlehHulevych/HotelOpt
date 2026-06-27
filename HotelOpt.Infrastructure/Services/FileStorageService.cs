using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HoteOpt.Application.Interfaces;

namespace HotelOpt.Infrastructure.Services;

public class FileStorageService:IFileStorageService
{
    private const string ContainerName = "avatars";
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;

    public FileStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
        _containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        _containerClient.CreateIfNotExists();
    }
    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, new BlobHttpHeaders{ContentType = contentType});
        return blobClient.Uri.ToString();

    }

    public async Task DeleteAsync(string fileUrl)
    {
        var blobName = new Uri(fileUrl).Segments.Last();
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DeleteAsync();
    }
}