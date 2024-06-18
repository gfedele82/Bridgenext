using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.DTO;
using Bridgenext.Models.Schema.DB;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Bridgenext.DataAccess.Repositories
{
    public class UserRepository (UserSystemContext _context): IUserRepository
    {
        private IDbContextTransaction? _transation;

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _context.Users
                .Include(x => x.Addreesses)
                .Include(x => x.UserTypes)
                .AsNoTracking().ToListAsync();

        }

        public async Task<PaginatedList<Users>> GetAllAsync(Pagination pagination)
        {
            var query = _context.Users.AsNoTracking();

            List<Users> items;
            int total = 0;

            if (string.IsNullOrWhiteSpace(pagination.Search))
            {
                items = await query
                   .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                   .Take(pagination.PageSize)
                   .Include(x => x.Addreesses)
                   .Include(x => x.UserTypes)
                   .ToListAsync();
                total = await query.CountAsync();
                return new PaginatedList<Users> { Items = items, Total = total };
            }

            var searchText = pagination.Search.ToLower();
            var searchTextPattern = $"%{searchText}%";
            var predicate = PredicateBuilder.New<Users>(true);

            Expression<Func<Users, bool>> CreatePredicateEmail() =>
                predicate.Or(x => EF.Functions.Like(x.Email.ToLower(), searchTextPattern.ToLower()));
            Expression<Func<Users, bool>> CreatePredicateCountry() =>
                predicate.Or(x => x.Addreesses.Any( p => p.Country.ToLower().Contains(searchTextPattern.ToLower())));
            Expression<Func<Users, bool>> CreatePredicateCity() =>
                predicate.Or(x => x.Addreesses.Any(p => p.City.ToLower().Contains(searchTextPattern.ToLower())));
            Expression<Func<Users, bool>> CreatePredicateZip() =>
                predicate.Or(x => x.Addreesses.Any(p => p.Zip.ToLower().Contains(searchTextPattern.ToLower())));

            var predicates = new Dictionary<string, Func<Expression<Func<Users, bool>>>> {
                { nameof(Users.Email).ToLower(), CreatePredicateEmail },
                { nameof(Addreesses.Country).ToLower(),  CreatePredicateCountry },
                { nameof(Addreesses.City).ToLower(),  CreatePredicateCity },
                { nameof(Addreesses.Zip).ToLower(),  CreatePredicateZip },
            };

            var defaultSearchFields = predicates.Keys.ToList();
            var searchFields = pagination.SearchFields.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var requiredSearchFields = searchFields.Any() ? searchFields : defaultSearchFields;
            foreach (var searchField in requiredSearchFields)
            {
                var createPredicate = predicates.GetValueOrDefault(searchField.ToLower());
                predicate = createPredicate == null ? predicate : createPredicate();
            }

            var whereStatement = query.Where(predicate);
            items = await whereStatement
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Include(x => x.Addreesses)
                .Include(x => x.UserTypes)
                .ToListAsync();
            total = await whereStatement.CountAsync();

            return new PaginatedList<Users> { Items = items, Total = total };
        }

        public async Task<IEnumerable<Users>> GetAllByEmail(string email)
        {
            return await _context.Users.AsNoTracking()
                .Include(x => x.Addreesses)
                .Include(x => x.UserTypes)
                .Where(x => x.Email.ToLower().Contains(email.ToLower())).ToListAsync();
        }

        public async Task<IEnumerable<Users>> GetByCriteria(Expression<Func<Users, bool>> predicateSearch) =>
            await _context.Users.Where(predicateSearch).AsNoTracking().Include(x => x.Addreesses).ToListAsync();

        public async Task<Users> GetAsync(Guid id) =>
            await _context.Users
                .Where(x => x.Id == id)
                .Include(x => x.Addreesses)
                .Include(x => x.UserTypes)
                .AsNoTracking()
                .FirstOrDefaultAsync();

        public async Task<Users> InsertAsync(Users user)
        {
            _context.ChangeTracker.Clear();
            _context.Entry(user.UserTypes).State = EntityState.Unchanged;

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<Users> UpdateAsync(Users user)
        {
            _context.ChangeTracker.Clear();

            _context.Users.Update(user);
            _context.Entry(user.UserTypes).State = EntityState.Unchanged;

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task DeleteAsync(Users user)
        {
            _context.ChangeTracker.Clear();

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IdExistsAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        public void InitialTransaction()
        {
            _transation = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transation != null)
            {
                _transation.Commit();
            }
        }

        public void FailTransaction()
        {
            if (_transation != null)
            {
                _transation.Rollback();
            }
        }
    }
}
