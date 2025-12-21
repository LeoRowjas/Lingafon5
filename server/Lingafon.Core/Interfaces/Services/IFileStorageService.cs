namespace Lingafon.Core.Interfaces.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string bucketName);
    Task DeleteFileAsync(string fileName, string bucketName);
}