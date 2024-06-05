using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema;

namespace Bridgenext.Engine.Interfaces
{
    public interface IDocumentEngine
    {
        Task<Documents> CreateDocument(CreateDocumentRequest addDocumentRequest);
    }
}
