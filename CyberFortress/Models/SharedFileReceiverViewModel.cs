using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberFortress.Models
{
    public class SharedFileReceiverViewModel
    {
        public int Id { get; set; }
        public int SharedFileId { get; set; }
        [Display(Name = "Share With")]
        public string SharedFileReceiverUsername { get; set; }

    }
}