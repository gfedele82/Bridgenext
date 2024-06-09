using Bridgenext.Models.DTO;
using Bridgenext.Models.Schema.DB;
using System.Linq.Expressions;


namespace Bridgenext.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAll();

        Task<PaginatedList<Users>> GetAllAsync(Pagination pagination);

        Task<Users> GetAsync(Guid id);

        Task<Users> InsertAsync(Users user);

        Task<Users> UpdateAsync(Users user);

        Task<IEnumerable<Users>> GetAllByEmail(string email);

        Task DeleteAsync(Users user);

        Task<bool> IdExistsAsync(string email);

        Task<IEnumerable<Users>> GetByCriteria(Expression<Func<Users, bool>> predicateSearch);

        void InitialTransaction();

        void CommitTransaction();

        void FailTransaction();
    }
}
