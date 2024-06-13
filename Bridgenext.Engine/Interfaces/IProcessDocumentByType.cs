using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;

namespace Bridgenext.Engine.Interfaces
{
    public interface IProcessDocumentByType
    {
        Task<Documents> CreateDocument(CreateDocumentRequest addDocumentRequest, Users user);

        Task<Documents> UpdateDocument(UpdateDocumentFileRequest updateDocumnetFileRequest, Users user, Documents existDocument);
    }
}
