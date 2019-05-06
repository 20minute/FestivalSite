using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Festival.Models;

namespace Festival.Models
{
    public class ControleFestivalier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public DateTime Naissance { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Mdp { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string CodePostal { get; set; }
        [Required]
        public string Ville { get; set; }
        [Required]
        public string Rue { get; set; }
        [Required]
        public string Pays { get; set; }
        [Required]
        public string Emailv { get; set; }
        [Required]
        public string Mdpv { get; set; }

        [ForeignKey("Festival")]
        public int FestivalId { get; set; }
        public virtual Festival Festival { get; set; }

        public Festivalier ConvertToFestivalier()
        {
            Festivalier festivaliers = new Festivalier(Nom, Prenom, Naissance, Email, Mdp,
            Genre, Telephone, CodePostal, Ville, Rue, Pays, FestivalId);
            return festivaliers;
        }
    }
}