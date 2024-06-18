namespace Bridgenext.Models.DTO.Request
{
    public class UpdateDocumentFileRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string File { get; set; }

        public string ModifyUser { get; set; }
    }
}
