namespace Bridgenext.Models.DTO.Request
{
    public class DisableDocumentRequest
    {
        public Guid Id { get; set; }

        public string ModifyUser { get; set; }

        public string Comment { get; set; }
    }
}
