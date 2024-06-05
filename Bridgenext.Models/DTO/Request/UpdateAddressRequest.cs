namespace Bridgenext.Models.DTO.Request
{
    public class UpdateAddressRequest
    {
        public Guid Id { get; set; }
        public string Line1 { get; set; }
        public string? Line2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string ModifyUser { get; set; }
    }
}
