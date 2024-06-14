namespace Bridgenext.Models.DTO.Request
{
    public class CreateCommetRequest
    {
        public Guid IdDocument { get; set; }

        public string Content { get; set; }

        public string CreateUser { get; set; }
    }
}
