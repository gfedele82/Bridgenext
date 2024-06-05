using System.ComponentModel.DataAnnotations;
using Bridgenext.Models.Schema.Base.Audit;

namespace Bridgenext.Models.Schema
{
    public class Addreesses_History : AuditableEntity
    {

        public Guid IdUser { get; set; }

        [Required]
        [MaxLength(250)]
        public string Line1 { get; set; }

        [MaxLength(250)]
        public string? Line2 { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        [MaxLength(20)]
        public string Zip { get; set; }

    }
}
