using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;

namespace Bridgenext.Engine.Interfaces
{
    public interface IDocumentEngine
    {
        Task<DocumentDto> CreateDocument(CreateDocumentRequest addDocumentRequest);

        Task<DocumentDto> GetDocumentById(Guid id);

        Task<Tuple<string, MemoryStream>> Download(Guid id);

        Task<List<DocumentSearchDto>> GetDocumentByText(string text);

        Task<DocumentDto> DisableDocument(DisableDocumentRequest disableDocumentRequest);

        Task<DocumentDto> ModifyDocument(UpdateDocumentRequest documentRequest);

        Task<DocumentDto> UpdateFileDocument(UpdateDocumentFileRequest updateDocumentFileRequest);

        Task<DocumentDto> DeleteDocument(DeleteDocumentRequest deleteDocument);
    }
}
