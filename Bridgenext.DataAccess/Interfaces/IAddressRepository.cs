using Bridgenext.Models.DTO;
using Bridgenext.Models.Schema.DB;
using System.Linq.Expressions;

namespace Bridgenext.DataAccess.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Addreesses>> GetAll();

        Task<PaginatedList<Addreesses>> GetAllAsync(Pagination pagination);

        Task<Addreesses> GetAsync(Guid id);

        Task<Addreesses> InsertAsync(Addreesses address);

        Task<Addreesses> UpdateAsync(Addreesses address);

        Task<IEnumerable<Addreesses>> GetAllByCountry(string country);

        Task<IEnumerable<Addreesses>> GetAllByCity(string city);

        Task<IEnumerable<Addreesses>> GetAllByZip(string zip);

        Task DeleteAsync(Addreesses address);

        Task<bool> IdExistsAsync(Guid Id);

        Task<IEnumerable<Addreesses>> GetByCriteria(Expression<Func<Addreesses, bool>> predicateSearch);
    }
}
