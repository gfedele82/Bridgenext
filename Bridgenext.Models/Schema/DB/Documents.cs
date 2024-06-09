using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Bridgenext.Models.Schema.DB.Base;

namespace Bridgenext.Models.Schema.DB
{
    public class Documents : Security
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
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

        public virtual DocumentsType DocumentType { get; set; } = new DocumentsType();

        public virtual Users Users { get; set; } = new Users();

        public List<Comments> Comments { get; set; } = new List<Comments>();


    }
}
