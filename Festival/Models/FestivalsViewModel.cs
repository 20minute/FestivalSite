using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Festival.Models
{
    public class FestivalsViewModel
    {
        public List<Festival> Festivals { get; set; }
        public List<Artiste> Artistes { get; set; }
        public List<Programmation> Programmations { get; set; }
        public List<Scene> Scenes { get; set; }
        public List<Festivalier> Festivaliers { get; set; }
        public List<Selection> Selections { get; set; }
    }
}