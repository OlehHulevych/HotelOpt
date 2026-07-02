using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HotelOpt.Application.Interfaces;

namespace HotelOpt.Infrastructure.Services;

public class FileStorageService:IFileStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public FileStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }
    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, new BlobHttpHeaders{ContentType = contentType});
        return blobClient.Uri.ToString();

    }

    public async Task DeleteAsync(string fileUrl,string containerName)
    {
        var blobName = new Uri(fileUrl).Segments.Last();
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteAsync();
    }
}