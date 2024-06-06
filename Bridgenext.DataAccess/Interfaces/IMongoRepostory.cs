using Bridgenext.Models.Schema;

namespace Bridgenext.DataAccess.Interfaces
{
    public interface IMongoRepostory
    {
        Task<Documents> CreateDocument(Documents document, string text);
    }
}
