namespace Lingafon.Core.Interfaces.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    Task DeleteFileAsync(string fileName);
}