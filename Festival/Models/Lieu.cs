using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class Lieu
    {
        [Key, ForeignKey("Festival")]
        public int FestivalId { get; set; }
        public string LieuName { get; set; }
        public int PostalCode { get; set; }


        public virtual Festival Festival { get; set; }
    }
}