﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionDepot.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = "";

        [Column(TypeName = "nvarchar(16)")]
        public string Adresse { get; set; } = "";

        [Column(TypeName = "nvarchar(16)")]
        public string Type { get; set; } = "";

        [Column(TypeName = "nvarchar(16)")]
        public string Cin { get; set; } = "";

        [ForeignKey("Societe")]
        public int? IdSociete { get; set; }
        public Societe Societe { get; set; }
    }
}
