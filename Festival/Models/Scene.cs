using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class Scene
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Capacite { get; set; }
        public string Accessibilite { get; set; }
    }
}