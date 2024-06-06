using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;

namespace Bridgenext.Engine.Interfaces
{
    public interface IDocumentEngine
    {
        Task<DocumentDto> CreateDocument(CreateDocumentRequest addDocumentRequest);
    }
}
