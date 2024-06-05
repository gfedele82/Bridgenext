
namespace Bridgenext.Models.DTO.Request
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int IdUserType { get; set; }
        public string CreateUser { get; set; }
        public List<CreateAddressRequest> Addresses { get; set; } = new List<CreateAddressRequest>(); 
    }
}
