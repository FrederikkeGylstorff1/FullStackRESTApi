using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestApiLK.data
{
    public class Kunder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KundeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Navn { get; set; }

        [Required]
        [StringLength(50)]
        public string Efternavn { get; set; }

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
        public string Email { get; set; }
    }
}
