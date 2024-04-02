using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestApiLK.data
{
    public class Ordre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrdreID { get; set; }

        public int KundeID { get; set; }

        public int ProduktID { get; set; }

        public int Antal { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Ordredato { get; set; }

        public string Ordrestatus { get; set; }

        [ForeignKey("KundeID")]
        public virtual Kunder Kunde { get; set; }

        [ForeignKey("ProduktID")]
        public virtual produkter Produkt { get; set; }
    }
}
