namespace Bridgenext.Models.DTO.Request
{
    public class UpdateDocumentRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string? Content { get; set; }

        public string ModifyUser { get; set; }
    }
}
