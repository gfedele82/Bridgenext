namespace Bridgenext.Models.DTO.Response
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyUser { get; set; }
        public DateTime ModifyDate { get; set; }
        public UserTypeDto UserType { get; set; } = new UserTypeDto();
        public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
    }
}
