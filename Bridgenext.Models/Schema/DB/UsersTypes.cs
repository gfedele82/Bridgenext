using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Bridgenext.Models.Schema.DB
{
    public class UsersTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Type { get; set; }

        public virtual List<Users> Users { get; set; } = new List<Users>();
    }
}
