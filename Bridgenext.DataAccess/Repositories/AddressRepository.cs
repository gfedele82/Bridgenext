using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.DTO;
using Bridgenext.Models.Schema;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bridgenext.DataAccess.Repositories
{
    public class AddressRepository (UserSystemContext _context) : IAddressRepository
    {
        public async Task<IEnumerable<Addreesses>> GetAll()
        {
            return await _context.Addreesses.AsNoTracking().ToListAsync();
        }

        public async Task<PaginatedList<Addreesses>> GetAllAsync(Pagination pagination)
        {
            var query = _context.Addreesses.AsNoTracking();

            List<Addreesses> items;
            int total = 0;

            if (string.IsNullOrWhiteSpace(pagination.Search))
            {
                items = await query
                   .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                   .Take(pagination.PageSize)
                   .Include(x => x.User)
                   .ToListAsync();
                total = await query.CountAsync();
                return new PaginatedList<Addreesses> { Items = items, Total = total };
            }

            var searchText = pagination.Search.ToLower();
            var searchTextPattern = $"%{searchText}%";
            var predicate = PredicateBuilder.New<Addreesses>(true);

            Expression<Func<Addreesses, bool>> CreatePredicateZip() =>
                predicate.Or(x => EF.Functions.Like(x.Zip, searchTextPattern));
            Expression<Func<Addreesses, bool>> CreatePredicateCountry() =>
                predicate.Or(x => EF.Functions.Like(x.Country, searchTextPattern));
            Expression<Func<Addreesses, bool>> CreatePredicateCity() =>
                predicate.Or(x => EF.Functions.Like(x.City, searchTextPattern));

            var predicates = new Dictionary<string, Func<Expression<Func<Addreesses, bool>>>> {
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
                .Include(x => x.User)
                .ToListAsync();
            total = await whereStatement.CountAsync();

            return new PaginatedList<Addreesses> { Items = items, Total = total };
        }

        public async Task<IEnumerable<Addreesses>> GetAllByCountry(string country)
        {
            return await _context.Addreesses.AsNoTracking().Include(x => x.User).Where(x => x.Country.ToLower().Contains(country.ToLower())).ToListAsync();
        }

        public async Task<IEnumerable<Addreesses>> GetAllByCity(string city)
        {
            return await _context.Addreesses.AsNoTracking().Include(x => x.User).Where(x => x.City.ToLower().Contains(city.ToLower())).ToListAsync();
        }

        public async Task<IEnumerable<Addreesses>> GetAllByZip(string zip)
        {
            return await _context.Addreesses.AsNoTracking().Include(x => x.User).Where(x => x.Zip.ToLower().Contains(zip.ToLower())).ToListAsync();
        }

        public async Task<IEnumerable<Addreesses>> GetByCriteria(Expression<Func<Addreesses, bool>> predicateSearch) =>
            await _context.Addreesses.Where(predicateSearch).AsNoTracking().Include(x => x.User).ToListAsync();

        public async Task<Addreesses> GetAsync(Guid id) =>
            await _context.Addreesses
                .Where(x => x.Id == id)
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync();

        public async Task<Addreesses> InsertAsync(Addreesses address)
        {
            _context.ChangeTracker.Clear();

            _context.Addreesses.Add(address);

            await _context.SaveChangesAsync();

            return address;
        }

        public async Task<Addreesses> UpdateAsync(Addreesses address)
        {
            _context.ChangeTracker.Clear();

            _context.Addreesses.Update(address);

            await _context.SaveChangesAsync();

            return address;
        }

        public async Task DeleteAsync(Addreesses address)
        {
            _context.ChangeTracker.Clear();

            _context.Addreesses.Remove(address);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IdExistsAsync(Guid Id)
        {
            return await _context.Addreesses
                .AsNoTracking()
                .AnyAsync(x => x.Id == Id);
        }
    }
}
