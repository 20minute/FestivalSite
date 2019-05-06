using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class ChiffreSelections
    {
        public Programmation Programmation;
        public int nbPersonnePri;
        public int nbPersonneSec;

        public ChiffreSelections(Programmation a, int nbPersonnePri, int nbPersonneSec)
        {
            Programmation = a;
            this.nbPersonnePri = nbPersonnePri;
            this.nbPersonneSec = nbPersonneSec;
        }
    }
}