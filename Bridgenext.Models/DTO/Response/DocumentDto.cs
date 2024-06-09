namespace Bridgenext.Models.DTO.Response
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string? Content { get; set; }

        public string? FileName { get; set; }

        public string? SourceFile { get; set; }

        public string? TargetFile { get; set; }

        public long? Size { get; set; }

        public bool Hide { get; set; }

        public string? MongoId { get; set; }

        public  DocumentTypeDto DocumentType { get; set; } = new DocumentTypeDto();

        public  UserDto Users { get; set; } = new UserDto();

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
