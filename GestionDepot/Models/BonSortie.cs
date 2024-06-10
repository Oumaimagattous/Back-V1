using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDepot.Models
{
    public class BonSortie
    {
        public int Id { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "decimal(16,3)")]
        public decimal Qte { get; set; }

        [ForeignKey("Client")]
        public int? IdClient { get; set; }
        public Client Client { get; set; }


        [ForeignKey("Produit")]
        public int? IdProduit { get; set; }
        public Produit Produit { get; set; }


        [ForeignKey("Chambre")]
        public int? IdChambre { get; set; }
        public Chambre Chambre { get; set; }


        [ForeignKey("Societe")]
        public int? IdSociete { get; set; }
        public Societe Societe { get; set; }



    }
}

