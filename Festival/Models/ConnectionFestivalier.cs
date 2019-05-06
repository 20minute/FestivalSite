using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Festival.Models
{
    public class ConnectionFestivalier
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Mdp { get; set; }
    }
}