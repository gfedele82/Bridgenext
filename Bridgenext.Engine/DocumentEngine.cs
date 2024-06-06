using Bridgenext.DataAccess.DTOAdapter;
using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Engine.Utils;
using Bridgenext.Models.Configurations;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Bridgenext.Models.Enums;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Bridgenext.Engine
{
    public class DocumentEngine (ILogger<DocumentEngine> _logger,
        DocumentTypeResolver _documentTypeResolver,
        IConfigurationRoot _configuration,
        IUserRepository _userRepository,
        IDocumentRepositoty _documentRepository,
        IValidator<CreateDocumentRequest> _addDocumentRequestValidator
        ) : IDocumentEngine
    {
        public async Task<DocumentDto> CreateDocument(CreateDocumentRequest addDocumentRequest)
        {
            _logger.LogInformation($"CreateDocument: Payload = {JsonConvert.SerializeObject(addDocumentRequest)}");

            await _addDocumentRequestValidator.ValidateAndThrowAsync(addDocumentRequest);

            var user = (await _userRepository.GetAllByEmail(addDocumentRequest.CreateUser)).FirstOrDefault();

            var selectType = FileTypes.Text;
            if ( addDocumentRequest.File != null)
            {
                var filextensionConfig = _configuration.GetSection("FilesExtension").Get<FilesExtensionSettings>();

                var fileExtension = Path.GetExtension(addDocumentRequest.File).ToUpper().Replace(".", "");

                if(filextensionConfig.Image.Contains(fileExtension))
                {
                    selectType = FileTypes.Image;
                }
                else if (filextensionConfig.Video.Contains(fileExtension))
                {
                    selectType = FileTypes.Video;
                }
                else
                {
                    selectType = FileTypes.Document;
                }

            }

            var response = await _documentTypeResolver(selectType).CreateDocument(addDocumentRequest, user);

            response = await _documentRepository.InsertAsync(response);

             return response.ToDomainModel();

        }
    }
}
