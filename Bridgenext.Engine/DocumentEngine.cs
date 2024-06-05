using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Engine.Utils;
using Bridgenext.Models.Configurations;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Bridgenext.Engine
{
    public class DocumentEngine (ILogger<DocumentEngine> _logger,
        DocumentTypeResolver _documentTypeResolver,
        IConfigurationRoot _configuration,
        IUserRepository _userRepository
        ) : IDocumentEngine
    {
        public async Task<Documents> CreateDocument(CreateDocumentRequest addDocumentRequest)
        {
            _logger.LogInformation($"CreateDocument: Payload = {JsonConvert.SerializeObject(addDocumentRequest)}");

            //await _addUserRequestValidator.ValidateAndThrowAsync(addUserRequest);

            var user = (await _userRepository.GetAllByEmail(addDocumentRequest.CreateUser)).FirstOrDefault();

            var filextensionConfig = _configuration.GetSection("FilesExtension").Get<FilesExtensionSettings>();

            var selectType = FileTypes.Document;

            var response = await _documentTypeResolver(selectType).CreateDocument(addDocumentRequest, user);


            return response;

            /* var dbUser = addUserRequest.ToDatabaseModel();

             dbUser = await _userRepository.InsertAsync(dbUser);

             return dbUser.ToDomainModel();*/
        }
    }
}
