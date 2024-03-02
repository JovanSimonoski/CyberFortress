using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberFortress.Models
{
    public class EncryptedPassword
    {
        public int Id { get; set; }
        public string Website { get; set; }
        public string Username { get; set; }
        [Display(Name = "Password")]
        public string EncryptedData { get; set; }
        public string UserId { get; set; }
    }
}