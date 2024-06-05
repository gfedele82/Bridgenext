namespace Bridgenext.Models.DTO.Request
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int IdUserType { get; set; }
        public string ModifyUser { get; set; }
        public List<UpdateAddressRequest> Addresses { get; set; } = new List<UpdateAddressRequest>();
    }
}
