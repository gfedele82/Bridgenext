using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bridgenext.Engine.Strategy
{
    public class DocumentProcessText(ILogger<DocumentProcessText> _logger) : IProcessDocumentByType
    {
        public async Task<Documents> CreateDocument(CreateDocumentRequest addDocumentRequest, Users user)
        {
            _logger.LogInformation($"DocumentProcessText: Payload = {JsonConvert.SerializeObject(addDocumentRequest)}");

            Documents _document = new Documents()
            {
                Content = addDocumentRequest.Content,
                CreateDate = DateTime.Now,
                CreateUser = addDocumentRequest.CreateUser,
                Description = addDocumentRequest.Description,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Text,
                    Type = Enum.GetName(typeof(FileTypes), FileTypes.Text)
                },
                FileName = null,
                Hide = false,
                Id = Guid.NewGuid(),
                IdDocumentType = (int)FileTypes.Text,
                Users = user,
                IdUser = user.Id,
                ModifyDate = DateTime.Now,
                ModifyUser = addDocumentRequest.CreateUser,
                Name = addDocumentRequest.Name,
                SourceFile = null,
                TargetFile = null,
                Size = null
            };

             return _document;

        }
    }
}
