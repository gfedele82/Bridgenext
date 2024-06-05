using Bridgenext.Models.Configurations;

namespace Bridgenext.Models.DTO
{
    public class Pagination
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;

        public string? Search { get; set; }

        public List<string> SearchFields { get; set; } = new();

        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                _pageNumber = value > 0 ? value : _pageNumber;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > SystemParameters.MaxPageSize) ? SystemParameters.MaxPageSize : (value <= 0 ? _pageSize : value);
            }
        }

    }
}
