using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bridgenext.Models.Schema.Base
{
    public class Security
    {
        [Required]
        [MaxLength(100)]
        public string CreateUser { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string ModifyUser { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ModifyDate { get; set; }
    }
}
