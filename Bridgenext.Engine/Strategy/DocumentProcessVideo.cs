using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.Configurations;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Newtonsoft.Json;

namespace Bridgenext.Engine.Strategy
{
    public class DocumentProcessVideo (ILogger<DocumentProcessVideo> _logger,
        IConfigurationRoot _configuration) : IProcessDocumentByType
    {
        private readonly string path = "Video";

        public async Task<Documents> CreateDocument(CreateDocumentRequest addDocumentRequest, Users user)
        {
            _logger.LogInformation($"DocumentProcessVideo: Payload = {JsonConvert.SerializeObject(addDocumentRequest)}");

            var minioConfig = _configuration.GetSection("Minio").Get<MinioSettings>();

            Documents _document = new Documents()
            {
                Content = addDocumentRequest.Content,
                CreateDate = DateTime.Now,
                CreateUser = addDocumentRequest.CreateUser,
                Description = addDocumentRequest.Description,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Video,
                    Type = Enum.GetName(typeof(FileTypes), FileTypes.Video)
                },
                FileName = Path.GetFileName(addDocumentRequest.File),
                Hide = false,
                Id = Guid.NewGuid(),
                IdDocumentType = (int)FileTypes.Video,
                Users = user,
                IdUser = user.Id,
                ModifyDate = DateTime.Now,
                ModifyUser = addDocumentRequest.CreateUser,
                Name = addDocumentRequest.Name,
                SourceFile = addDocumentRequest.File,
                TargetFile = $"{path}/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(addDocumentRequest.File)}"
            };

            using (var _minioClient = new MinioClient().WithEndpoint(minioConfig.EndPoint)
                  .WithCredentials(minioConfig.AccessKey, minioConfig.SecretKey)
                  .WithSSL(minioConfig.SSL).Build())
            {

                try
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
                catch (Exception ex)
                {
                    _logger.LogError($"CreateDocument: Payload = {JsonConvert.SerializeObject(addDocumentRequest)} - Error = {ex.Message}");

                    return null;
                }

                return _document;

            }

        }
    }
}
