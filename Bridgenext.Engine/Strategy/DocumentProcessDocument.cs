using Bridgenext.DataAccess.Interfaces;
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

namespace Bridgenext.Engine.Strategy
{
    public class DocumentProcessDocument (ILogger<DocumentProcessImage> _logger,
        IConfigurationRoot _configuration,
        IMongoRepostory _mongoRepository): IProcessDocumentByType
    {
        private readonly string path = "Document";

        public async Task<Documents> CreateDocument(CreateDocumentRequest addDocumentRequest, Users user)
        {
            _logger.LogInformation($"DocumentProcessDocument: Payload = {JsonConvert.SerializeObject(addDocumentRequest)}");

            var minioConfig = _configuration.GetSection("Minio").Get<MinioSettings>();

            Documents _document = new Documents()
            {
                Content = addDocumentRequest.Content,
                CreateDate = DateTime.Now,
                CreateUser = addDocumentRequest.CreateUser,
                Description = addDocumentRequest.Description,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Document,
                    Type = Enum.GetName(typeof(FileTypes), FileTypes.Document)
                },
                FileName = Path.GetFileName(addDocumentRequest.File),
                Hide = false,
                Id = Guid.NewGuid(),
                IdDocumentType = (int)FileTypes.Document,
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

                var fileContent = File.ReadAllText(_document.SourceFile);

                _document =await  _mongoRepository.CreateDocument(_document, fileContent);

                return _document;

            }
        }
    }
}
