using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestApiLK.data
{
    public class Forhandler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForhandlerID { get; set; }

        [Required]
        [StringLength(100)]
        public string Navn { get; set; }

        [Required]
        [StringLength(100)]
        public string Adresse { get; set; }

        [Required]
        [StringLength(50)]
        public string PostnummerBy { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefon { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Mail { get; set; }

        [StringLength(100)]
        public string Åbningstider { get; set; }

        // Latitude (breddegrad)
        public double Latitude { get; set; }

        // Longitude (længdegrad)
        public double Longitude { get; set; }
    }
}
