namespace Bridgenext.Models.DTO.Request
{
    public class CreateAddressRequest
    {
        public string Line1 { get; set; }
        public string? Line2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string CreateUser { get; set; }
    }
}
