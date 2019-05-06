using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Festival.Models
{
    public class Selection
    {
        public int SelectionId { get; set; }

        [ForeignKey("Programmation"), ]
        public int ProgrammationId { get; set; }
        public virtual Programmation Programmation { get; set; }

        [ForeignKey("Festivalier")]
        public int FestivalierId { get; set; }
        public virtual Festivalier Festivalier { get; set; }

        public int PrimaireSecondaire { get; set; }

    }

}