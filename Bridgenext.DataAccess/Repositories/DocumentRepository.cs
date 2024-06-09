using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.DTO;
using Bridgenext.Models.Schema.DB;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace Bridgenext.DataAccess.Repositories
{
    public class DocumentRepository (UserSystemContext _context) : IDocumentRepositoty
    {
        public async Task<IEnumerable<Documents>> GetAll()
        {
            return await _context.Documents.AsNoTracking().ToListAsync();
        }

        public async Task<PaginatedList<Documents>> GetAllAsync(Pagination pagination)
        {
            var query = _context.Documents.AsNoTracking();

            List<Documents> items;
            int total = 0;

            if (string.IsNullOrWhiteSpace(pagination.Search))
            {
                items = await query
                   .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                   .Take(pagination.PageSize)
                   .Include(x => x.Users)
                   .Include(x => x.DocumentType)
                   .ToListAsync();
                total = await query.CountAsync();
                return new PaginatedList<Documents> { Items = items, Total = total };
            }

            var searchText = pagination.Search.ToLower();
            var searchTextPattern = $"%{searchText}%";
            var predicate = PredicateBuilder.New<Documents>(true);

            Expression<Func<Documents, bool>> CreatePredicateDocumentName() =>
                predicate.Or(x => EF.Functions.Like(x.Name, searchTextPattern));
            Expression<Func<Documents, bool>> CreatePredicateDocumentDescription() =>
                predicate.Or(x => EF.Functions.Like(x.Description, searchTextPattern));

            var predicates = new Dictionary<string, Func<Expression<Func<Documents, bool>>>> {
                { nameof(Documents.Name).ToLower(),  CreatePredicateDocumentName },
                { nameof(Documents.Description).ToLower(),  CreatePredicateDocumentDescription },
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
                .Include(x => x.Users)
                .Include(x => x.DocumentType)
                .ToListAsync();
            total = await whereStatement.CountAsync();

            return new PaginatedList<Documents> { Items = items, Total = total };
        }

        public async Task<Documents> GetAsync(Guid id) =>
            await _context.Documents
                .Where(x => x.Id == id)
                .Include(x => x.Users)
                .Include(x => x.DocumentType)
                .AsNoTracking()
                .FirstOrDefaultAsync();

        public async Task<Documents> InsertAsync(Documents document)
        {
            _context.ChangeTracker.Clear();

            _context.Documents.Add(document);
            _context.Entry(document.Users).State = EntityState.Unchanged;
            _context.Entry(document.DocumentType).State = EntityState.Unchanged;
            _context.Entry(document.Users.UserTypes).State = EntityState.Unchanged;

            await _context.SaveChangesAsync();

            return document;
        }

        public async Task<Documents> UpdateAsync(Documents document)
        {
            _context.ChangeTracker.Clear();
            _context.Entry(document.Users).State = EntityState.Unchanged;
            _context.Entry(document.DocumentType).State = EntityState.Unchanged;
            _context.Entry(document.Users.UserTypes).State = EntityState.Unchanged;

            _context.Documents.Update(document);

            await _context.SaveChangesAsync();

            return document;
        }

        public async Task<IEnumerable<Documents>> GetAllByUser(string user)
        {
            return await _context.Documents.AsNoTracking()
                .Include(x => x.Users)
                .Include(x => x.DocumentType)
                .Where(x => x.Users.Email.ToLower()
                .Contains(user.ToLower()))
                .ToListAsync();
        }

        public async Task DeleteAsync(Documents document)
        {
            _context.ChangeTracker.Clear();

            _context.Documents.Remove(document);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IdExistsAsync(Guid Id)
        {
            return await _context.Documents
                .AsNoTracking()
                .AnyAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<Documents>> GetByCriteria(Expression<Func<Documents, bool>> predicateSearch) =>
            await _context.Documents.Where(predicateSearch).AsNoTracking().Include(x => x.Users).Include(x => x.DocumentType).ToListAsync();
    }
}
