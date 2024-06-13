using Bridgenext.Models.Schema.DB;

namespace Bridgenext.Engine.Interfaces.Providers
{
    public interface IMinioEngine
    {
        Task<Documents> PutFile(Documents _document);

        Task DeleteFile(Documents _document);
    }
}
