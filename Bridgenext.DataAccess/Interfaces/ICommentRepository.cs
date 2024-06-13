using Bridgenext.Models.Schema.DB;
using System.Linq.Expressions;

namespace Bridgenext.DataAccess.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comments>> GetAll();

        Task<Comments> InsertAsync(Comments comment);

        Task<IEnumerable<Comments>> GetByCriteria(Expression<Func<Comments, bool>> predicateSearch);

        Task DeleteAsync(Comments comment);
    }
}
