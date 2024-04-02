using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestApiLK.data
{
    public class Betaling
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BetalingsID { get; set; }

        public int OrdreID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Betalingsdato { get; set; }

        [Required]
        [StringLength(50)]
        public string Betalingsmetode { get; set; }

        public string Betalingsstatus { get; set; }

        [ForeignKey("OrdreID")]
        public virtual Ordre Ordre { get; set; }
    }
}
