namespace HotelOpt.Application.Interfaces;

public interface IFileStorageService
{
    public Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string containerName);
    public Task DeleteAsync(string fileUrl, string containerName);
}