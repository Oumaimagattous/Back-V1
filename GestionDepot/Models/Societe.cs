﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionDepot.Models
{
    public class Societe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; } = "";

        [Column(TypeName = "nvarchar(250)")]
        public string Adresse { get; set; } = "";

        [Column(TypeName = "nvarchar(16)")]
        public string MF { get; set; } = "";

        public ICollection<Client>? Clients { get; set; }
        public ICollection<Fournisseur>? Fournisseurs { get; set; }
    }
}
