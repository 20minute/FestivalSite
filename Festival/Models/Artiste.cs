using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class Artiste
    {
        public int ArtisteID { get; set; }
        public string ArtisteNom { get; set; }
        public string Photo { get; set; }
        public string Style { get; set; }
        public string Comment { get; set; }
        public string Nationality { get; set; }
        public string MusicExtract { get; set; }
        public virtual ICollection<Programmation> Programmations { get; set; }
    }
}