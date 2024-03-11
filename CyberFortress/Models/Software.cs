using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberFortress.Models
{
    public class Software
    {
        public int SoftwareId { get; set; }
        [Display(Name = "Software")]
        public string SoftwareName { get; set; }
        [Display(Name = "Description")]
        public string SoftwareDescription { get; set; }
        public string SoftwareImage { get; set; }
    }
}