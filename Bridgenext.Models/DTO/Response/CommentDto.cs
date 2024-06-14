namespace Bridgenext.Models.DTO.Response
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid IdDoc { get; set; }
        public string Comment { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
