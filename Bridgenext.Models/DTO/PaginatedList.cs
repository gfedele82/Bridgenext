namespace Bridgenext.Models.DTO
{
    public class PaginatedList<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
