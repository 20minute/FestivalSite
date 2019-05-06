using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class Interet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Interesser { get; set; }

        [ForeignKey("Festival")]
        public int FestivalId { get; set; }
        public virtual Festival Festival { get; set; }
    }
}