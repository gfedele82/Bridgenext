using Bridgenext.Engine.Interfaces.Providers;
using Bridgenext.Models.Configurations;
using Bridgenext.Models.Schema.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Newtonsoft.Json;

namespace Bridgenext.Engine.Providers
{
    public class MinioEngine (ILogger<MinioEngine> _logger,
        IConfigurationRoot _configuration) : IMinioEngine
    {

        public async Task<Documents> PutFile(Documents _document)
        {
            _logger.LogInformation($"PutFile  Payload : {JsonConvert.SerializeObject(_document)}");

            var minioConfig = _configuration.GetSection("Minio").Get<MinioSettings>();

            using (var _minioClient = new MinioClient().WithEndpoint(minioConfig.EndPoint)
              .WithCredentials(minioConfig.AccessKey, minioConfig.SecretKey)
              .WithSSL(minioConfig.SSL).Build())
            {
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(minioConfig.BucketName));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(minioConfig.BucketName));
                }

                var putRequest = new PutObjectArgs()
                    .WithBucket(minioConfig.BucketName)
                     .WithObject(_document.TargetFile)
                     .WithFileName(_document.SourceFile);

                var respose = await _minioClient.PutObjectAsync(putRequest);
                    _document.Size = respose.Size;
            }

            return _document;
        }

        public async Task DeleteFile(Documents _document)
        {
            _logger.LogInformation($"DeleteFile  Payload : {JsonConvert.SerializeObject(_document)}");

            var minioConfig = _configuration.GetSection("Minio").Get<MinioSettings>();

            using (var _minioClient = new MinioClient().WithEndpoint(minioConfig.EndPoint)
              .WithCredentials(minioConfig.AccessKey, minioConfig.SecretKey)
              .WithSSL(minioConfig.SSL).Build())
            {

                var remove = new RemoveObjectArgs()
                .WithBucket(minioConfig.BucketName)
                .WithObject(_document.TargetFile);

                await _minioClient.RemoveObjectAsync(remove);
            }

        }

        public async Task<MemoryStream> GetDownload(Documents _document)
        {
            _logger.LogInformation($"GetDownload Payload : {JsonConvert.SerializeObject(_document)}"); 

            var minioConfig = _configuration.GetSection("Minio").Get<MinioSettings>();

            using (var _minioClient = new MinioClient().WithEndpoint(minioConfig.EndPoint)
                    .WithCredentials(minioConfig.AccessKey, minioConfig.SecretKey)
                    .WithSSL(minioConfig.SSL).Build())
            {

                var memoryStream = new MemoryStream();

                await _minioClient.GetObjectAsync(new GetObjectArgs()
                    .WithBucket(minioConfig.BucketName)
                    .WithObject(_document.TargetFile)
                    .WithCallbackStream(stream =>
                    {
                        stream.CopyTo(memoryStream);
                    }));

                memoryStream.Position = 0;

                return memoryStream;

            }
        }
    }
}
