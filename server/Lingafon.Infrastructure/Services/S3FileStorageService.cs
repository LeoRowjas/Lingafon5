using Amazon.S3;
using Amazon.S3.Model;
using Lingafon.Core.Interfaces.Services;
using Lingafon.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Lingafon.Infrastructure.Services;

public class S3FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _client;
    private readonly S3Settings _settings;
    
    public S3FileStorageService(IOptions<S3Settings> s3Settings)
    {
        _settings = s3Settings.Value;
        var config = new AmazonS3Config()
        {
            ServiceURL = _settings.ServiceUrl,
            ForcePathStyle = true
        };
        
        _client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, config);
    }
    
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = contentType,
            AutoCloseStream = true
        };

        await _client.PutObjectAsync(putRequest);
        return $"{_settings.ServiceUrl}/{_settings.BucketName}/{fileName}";
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = fileName
        };

        await _client.DeleteObjectAsync(deleteRequest);
    }
}