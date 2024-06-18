using Bridgenext.DataAccess.DTOAdapter;
using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Engine.Utils;
using Bridgenext.Models.Configurations;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Bridgenext.Models.Enums;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;


namespace Bridgenext.Engine
{
    public class DocumentEngine (ILogger<DocumentEngine> _logger,
        DocumentTypeResolver _documentTypeResolver,
        IConfigurationRoot _configuration,
        IUserRepository _userRepository,
        IMongoRepostory _mongoRepository,
        ICommentRepository _commentRepository,
        IDocumentRepositoty _documentRepository,
        IValidator<CreateDocumentRequest> _addDocumentRequestValidator,
        IValidator<Guid> _downloadRequestValidator,
        IValidator<DisableDocumentRequest> _disabledocumentValidator,
        IValidator<UpdateDocumentRequest> _updateDocumentValidator,
        IValidator<UpdateDocumentFileRequest> _updateDocumentFileValidator,
        IValidator<DeleteDocumentRequest> _deleteDocumentValidator
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

            if (response != null)
            {
                response = await _documentRepository.InsertAsync(response);

                return response.ToDomainModel();
            }
            else
            {
                throw new ApplicationException(GeneralExceptions.SystemFail);
            }

        }

        public async Task<DocumentDto> UpdateFileDocument(UpdateDocumentFileRequest updateDocumentFileRequest)
        {
            _logger.LogInformation($"UpdateFileDocument: Payload = {JsonConvert.SerializeObject(updateDocumentFileRequest)}");

            await _updateDocumentFileValidator.ValidateAndThrowAsync(updateDocumentFileRequest);

            var user = (await _userRepository.GetAllByEmail(updateDocumentFileRequest.ModifyUser)).FirstOrDefault();

            var selectType = FileTypes.Text;

            var filextensionConfig = _configuration.GetSection("FilesExtension").Get<FilesExtensionSettings>();

            var fileExtension = Path.GetExtension(updateDocumentFileRequest.File).ToUpper().Replace(".", "");

            if (filextensionConfig.Image.Contains(fileExtension))
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

            var existDocument = await _documentRepository.GetAsync(updateDocumentFileRequest.Id);

            var response = await _documentTypeResolver(selectType).UpdateDocument(updateDocumentFileRequest, user, existDocument);

            if (response != null)
            {
                response = await _documentRepository.UpdateAsync(response);

                return response.ToDomainModel();
            }
            else
            {
                throw new ApplicationException(GeneralExceptions.SystemFail);
            }

        }

        public async Task<DocumentDto> ModifyDocument(UpdateDocumentRequest documentRequest)
        {
            _logger.LogInformation($"ModifyDocument: Payload = {JsonConvert.SerializeObject(documentRequest)}");

            await _updateDocumentValidator.ValidateAndThrowAsync(documentRequest);

            var existDocument = await _documentRepository.GetAsync(documentRequest.Id);

            var dbDocument = documentRequest.ToDatabaseModel(existDocument);

            existDocument = await _documentRepository.UpdateAsync(dbDocument);

            return existDocument.ToDomainModel();
        }

        public async Task<DocumentDto> DisableDocument(DisableDocumentRequest disableDocumentRequest)
        {
            _logger.LogInformation($"DisableDocument: Payload = {JsonConvert.SerializeObject(disableDocumentRequest)}");

            await _disabledocumentValidator.ValidateAndThrowAsync(disableDocumentRequest);

            var existDocument = (await _documentRepository.GetAsync(disableDocumentRequest.Id));

            var existUser = (await _userRepository.GetAllByEmail(disableDocumentRequest.ModifyUser)).FirstOrDefault();

            var dbDocument = disableDocumentRequest.ToDatabaseModel(existDocument);

            dbDocument = await _documentRepository.UpdateAsync(dbDocument);

            var dbComment = disableDocumentRequest.ToDatabaseModel(existDocument, existUser);

            await _commentRepository.InsertAsync(dbComment).ConfigureAwait(false);

            return dbDocument.ToDomainModel();
        }

        public async Task<DocumentDto> DeleteDocument(DeleteDocumentRequest deleteDocument)
        {
            await _deleteDocumentValidator.ValidateAndThrowAsync(deleteDocument);

            _logger.LogInformation($"DeleteDocument: Payload = {JsonConvert.SerializeObject(deleteDocument)}");

            var existingDocument = await _documentRepository.GetAsync(deleteDocument.Id);

            if (existingDocument == null)
                return null;

            var selectType = (FileTypes)existingDocument.DocumentType.Id;

            existingDocument = await _documentTypeResolver(selectType).DeleteDocument(deleteDocument,existingDocument);

            await _documentRepository.DeleteAsync(existingDocument);

            return existingDocument.ToDomainModel();
        }

        public async Task<DocumentDto> GetDocumentById(Guid id)
        {
            _logger.LogInformation($"DocumentDto: Id = {id}");

            var dbDocument = await _documentRepository.GetAsync(id);

            return dbDocument.ToDomainModel();

        }

        public async Task<GetPaginatedResponse<DocumentDto>> GetAllDocument(Pagination pagination)
        {
            _logger.LogInformation("GetAllDocument");

            var paginatedList = await _documentRepository.GetAllAsync(pagination);

            return new GetPaginatedResponse<DocumentDto>
            {
                Total = paginatedList.Total,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                Items = paginatedList.Items.ToDomainModel()
            };
        }

        public async Task<List<DocumentSearchDto>> GetDocumentByText(string text)
        {
            _logger.LogInformation($"GetDocumentByText: text = {text}");

            if (string.IsNullOrEmpty(text))
                return null;

            List<DocumentSearchDto> response = new List<DocumentSearchDto>();

            var dbDocument = await _documentRepository.GetByCriteria(x => !string.IsNullOrEmpty(x.Content) && x.Content.ToLower().Contains(text.ToLower()));

            if (dbDocument.Count() > 0)
            {
                response.AddRange(dbDocument.ToDomainSearchModel());
            }

            var result = await _mongoRepository.SearchByText(text);

            if (result != null && result.Count() > 0)
            {
                var ids = result.Select(x => Guid.Parse(x.IdDb)).ToList();

                dbDocument = await _documentRepository.GetByCriteria(x => ids.Contains(x.Id));

                foreach (var dbDoc in dbDocument)
                {
                    dbDoc.Content = result.FirstOrDefault(x => x.IdDb == dbDoc.Id.ToString()).content;

                    response.Add(dbDoc.ToDomainSearchModel());
                }
            }

            return response;
        }

        public async Task<Tuple<string,MemoryStream>> Download(Guid id)
        {
            _logger.LogInformation($"Download: Payload = {JsonConvert.SerializeObject(id)}");

            await _downloadRequestValidator.ValidateAndThrowAsync(id);

            var dbDocument = await _documentRepository.GetAsync(id);

            var selectType = (FileTypes)dbDocument.IdDocumentType;

            var response = await _documentTypeResolver(selectType).Download(dbDocument);

            if (response != null)
            {
                return Tuple.Create(dbDocument.FileName, response);
            }
            else
            {
                throw new ApplicationException(GeneralExceptions.SystemFail);
            }

        }
    }
}
