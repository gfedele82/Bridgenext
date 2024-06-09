using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Bridgenext.Models.Schema.DB
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid IdUser { get; set; }

        [Required]
        public Guid IdDocumnet { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Comment { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public virtual Users Users { get; set; } = new Users();

        public virtual Documents Documents { get; set; } = new Documents();

    }
}
