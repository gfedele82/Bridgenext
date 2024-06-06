namespace Bridgenext.Models.DTO.Request
{
    public class CreateDocumentRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string? Content { get; set; }

        public string? File { get; set; }

        public string CreateUser { get; set; }
    }
}
