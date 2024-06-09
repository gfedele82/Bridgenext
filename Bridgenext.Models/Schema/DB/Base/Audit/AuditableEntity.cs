using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Bridgenext.Models.Schema.DB.Base;

namespace Bridgenext.Models.Schema.DB.Base.Audit
{
    public class AuditableEntity : Security
    {
        public Guid Id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AuditId { get; set; }

        [MaxLength(100)]
        [Required]
        public string AuditAction { get; set; }

        [Column(TypeName = "date")]
        public DateTime AuditDate { get; set; }
    }
}
