using Bridgenext.Models.Schema.Base.Audit;
using System.ComponentModel.DataAnnotations;


namespace Bridgenext.Models.Schema
{
    public class Documents_History : AuditableEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        public string? Content { get; set; }

        public string? FileName { get; set; }

        public string? SourceFile { get; set; }

        public string? TargetFile { get; set; }

        public long? Size { get; set; }

        [Required]
        public bool Hide { get; set; }

        [Required]
        public int IdDocumentType { get; set; }

        [Required]
        public Guid IdUser { get; set; }

    }
}
