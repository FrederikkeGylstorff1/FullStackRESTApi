using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestApiLK.data
{
    public class produkter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProduktID { get; set; }

        [Required]
        [StringLength(100)]
        public string Titel { get; set; }

        public string Beskrivelse { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Pris { get; set; }

        public int AntalPåLager { get; set; }

        public string Indhold { get; set; }

        public string ImageUrl { get; set; }
    }
}
