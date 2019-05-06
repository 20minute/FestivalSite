using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class Programmation
    {
        public int ProgrammationId { get; set; }

        [ForeignKey("Festival")]
        public int FestivalID { get; set; }
        public virtual Festival Festival { get; set; }

        [ForeignKey("Artiste")]
        public int ArtisteID { get; set; }
        public virtual Artiste Artiste { get; set; }

        [ForeignKey("Scene")]
        public int SceneID { get; set; }
        public virtual Scene Scene { get; set; }

        [ForeignKey("Organisateur")]
        public int OrganisateurID { get; set; }
        public virtual Organisateur Organisateur { get; set; }

        [Required]
        public DateTime DateDebutConcert { get; set; }
        [Required]
        public DateTime DateFinConcert { get; set; }
    }
}