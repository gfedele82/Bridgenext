using Bridgenext.Models.Schema.DB;

namespace Bridgenext.DataAccess.Interfaces
{
    public interface IMongoRepostory
    {
        Task<bool> CreateDocument(Documents document, string text);
    }
}
