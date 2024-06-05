namespace Bridgenext.Models.DTO.Response
{
    public class GetPaginatedResponse<T>
    {
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
