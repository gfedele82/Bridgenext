using Bridgenext.Models.Schema.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bridgenext.Models.Schema
{
    public class Addreesses : Security
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid IdUser {  get; set; }

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

        public virtual Users User { get; set; }
    }
}
