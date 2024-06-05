using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.Configurations;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Newtonsoft.Json;
using System.IO;

namespace Bridgenext.Engine.Strategy
{
    public class DocumentProcessImage(ILogger<DocumentProcessImage> _logger,
        IConfigurationRoot _configuration) : IProcessDocumentByType
    {
        private readonly string path = "Image";

        public async Task<Documents> CreateDocument(CreateDocumentRequest addDocumentRequest, Users user)
        {
            _logger.LogInformation($"DocumentProcessImage: Payload = {JsonConvert.SerializeObject(addDocumentRequest)}");

            var minioConfig = _configuration.GetSection("Minio").Get<MinioSettings>();

            Documents _document = new Documents()
            {
                Context = addDocumentRequest.Context,
                CreateDate = DateTime.Now,
                CreateUser = addDocumentRequest.CreateUser,
                Description = addDocumentRequest.Description,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Image,
                    Type = Enum.GetName(typeof(FileTypes), FileTypes.Image)
                },
                FileName = Path.GetFileName(addDocumentRequest.File),
                Hide = false,
                Id = Guid.NewGuid(),
                IdDocumentType = (int)FileTypes.Image,
                Users = user,
                IdUser = user.Id,
                ModifyDate = DateTime.Now,
                ModifyUser = addDocumentRequest.CreateUser,
                Name = addDocumentRequest.Name,
                SourceFile = addDocumentRequest.File,
                TargetFile = $"{path}/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMdd")}_{Path.GetFileName(addDocumentRequest.File)}"
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
