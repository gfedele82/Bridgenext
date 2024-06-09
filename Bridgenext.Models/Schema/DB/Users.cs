using Bridgenext.Models.Schema.DB.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bridgenext.Models.Schema.DB
{
    public class Users : Security
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        public int IdUserType { get; set; }

        public virtual UsersTypes UserTypes { get; set; } = new UsersTypes();

        public virtual List<Addreesses> Addreesses { get; set; } = new List<Addreesses>();

        public virtual List<Comments> Comments { get; set; } = new List<Comments>();

        public virtual List<Documents> Documents { get; set; } = new List<Documents>();
    }
}
