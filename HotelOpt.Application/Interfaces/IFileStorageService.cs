namespace HoteOpt.Application.Interfaces;

public interface IFileStorageService
{
    public Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    public Task DeleteAsync(string fileUrl);
}