using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
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

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string bucketName)
    {
        // Ensure bucket exists in S3/MinIO. If not, try to create it.
        try
        {
            var exists = await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName);
            if (!exists)
            {
                var putBucketRequest = new PutBucketRequest { BucketName = bucketName };
                await _client.PutBucketAsync(putBucketRequest);
            }
        }
        catch
        {
            // ignore bucket creation errors here; PutObject will fail with meaningful message
        }

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = contentType,
            AutoCloseStream = true
        };

        await _client.PutObjectAsync(putRequest);
        // return URL using the actual bucket name used for upload
        return $"{_settings.ServiceUrl}/{bucketName}/{fileName}";
    }

    public async Task DeleteFileAsync(string fileName, string bucketName)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = fileName
        };

        await _client.DeleteObjectAsync(deleteRequest);
    }
}