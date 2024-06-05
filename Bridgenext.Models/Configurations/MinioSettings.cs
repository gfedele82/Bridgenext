namespace Bridgenext.Models.Configurations
{
    public class MinioSettings
    {
        public const string KEY = "Minio";
        public string EndPoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public bool SSL { get; set; }
        public string BucketName { get; set; }
    }
}
