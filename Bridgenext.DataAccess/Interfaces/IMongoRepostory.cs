using Bridgenext.Models.Schema.DB;
using Bridgenext.Models.Schema.NotSQL;

namespace Bridgenext.DataAccess.Interfaces
{
    public interface IMongoRepostory
    {
        Task<bool> CreateDocument(Documents document, string text);

        Task<List<MongoDocuments>> SearchByText(string text);
    }
}
