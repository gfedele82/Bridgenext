using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Schema.DB;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace Bridgenext.DataAccess.Repositories
{
    public class CommentRepository (UserSystemContext _context) : ICommentRepository
    {
        public async Task<IEnumerable<Comments>> GetAll()
        {
            return await _context.Comments.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Comments>> GetByCriteria(Expression<Func<Comments, bool>> predicateSearch) =>
            await _context.Comments.Where(predicateSearch).AsNoTracking().ToListAsync();

        public async Task<Comments> InsertAsync(Comments comment)
        {
            _context.ChangeTracker.Clear();

            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task DeleteAsync(Comments comment)
        {
            _context.ChangeTracker.Clear();

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
        }
    }
}
