using Bridgenext.Engine.Interfaces;
using Bridgenext.Engine.Interfaces.Providers;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema.DB;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bridgenext.Engine.Strategy
{
    public class DocumentProcessImage(ILogger<DocumentProcessImage> _logger,
        IMinioEngine _minioEngine) : IProcessDocumentByType
    {
        private readonly string path = "Image";

        public async Task<Documents> CreateDocument(CreateDocumentRequest addDocumentRequest, Users user)
        {
            _logger.LogInformation($"DocumentProcessImage: Payload = {JsonConvert.SerializeObject(addDocumentRequest)}");

            Documents _document = new Documents()
            {
                Content = addDocumentRequest.Content,
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
                TargetFile = $"{path}/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(addDocumentRequest.File)}"
            };


            try
            {
               _document = await _minioEngine.PutFile(_document);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateDocument: Payload = {JsonConvert.SerializeObject(addDocumentRequest)} - Error = {ex.Message}");

                return null;
            }

            return _document;

        }

        public async Task<Documents> UpdateDocument(UpdateDocumentFileRequest updateDocumnetFileRequest, Users user, Documents existDocument)
        {
            _logger.LogInformation($"UpdateDocument: Payload = {JsonConvert.SerializeObject(updateDocumnetFileRequest)}");

            await _minioEngine.DeleteFile(existDocument);

            existDocument.ModifyUser = updateDocumnetFileRequest.ModifyUser;
            existDocument.ModifyDate = DateTime.Now;
            existDocument.FileName = Path.GetFileName(updateDocumnetFileRequest.File);
            existDocument.DocumentType.Id = (int)FileTypes.Image;
            existDocument.DocumentType.Type = Enum.GetName(typeof(FileTypes), FileTypes.Image);
            existDocument.SourceFile = updateDocumnetFileRequest.File;
            existDocument.TargetFile = $"{path}/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(updateDocumnetFileRequest.File)}";

            try
            {
                existDocument = await _minioEngine.PutFile(existDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateDocument: Payload = {JsonConvert.SerializeObject(updateDocumnetFileRequest)} - Error = {ex.Message}");

                return null;
            }

            return existDocument;
        }
    }
}
