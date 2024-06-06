using Bridgenext.Models.DTO;
using Bridgenext.Models.Schema;
using System.Linq.Expressions;


namespace Bridgenext.DataAccess.Interfaces
{
    public interface IDocumentRepositoty
    {
        Task<IEnumerable<Documents>> GetAll();

        Task<PaginatedList<Documents>> GetAllAsync(Pagination pagination);

        Task<Documents> GetAsync(Guid id);

        Task<Documents> InsertAsync(Documents document);

        Task<Documents> UpdateAsync(Documents document);

        Task<IEnumerable<Documents>> GetAllByUser(string user);

        Task DeleteAsync(Documents document);

        Task<bool> IdExistsAsync(Guid Id);

        Task<IEnumerable<Documents>> GetByCriteria(Expression<Func<Documents, bool>> predicateSearch);
    }
}
