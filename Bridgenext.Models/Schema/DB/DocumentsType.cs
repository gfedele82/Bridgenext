using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bridgenext.Models.Schema.DB
{
    public class DocumentsType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Type { get; set; }

        public virtual List<Documents> Documents { get; set; } = new List<Documents>();
    }
}
