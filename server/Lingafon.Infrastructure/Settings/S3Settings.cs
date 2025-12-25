namespace Lingafon.Infrastructure.Settings;

public class S3Settings
{
    public string ServiceUrl { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketNameAvatars { get; set; } = string.Empty;
    public string BucketNameAudio { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}