using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberFortress.Models
{
    public class SharedFile
    {
        public int Id { get; set; }
        [Display(Name = "File")]
        [Required]
        public string SharedFileName { get; set; }
        [Required]
        public string SharedFilePath { get; set; }
        [Required]
        public string SenderUserId { get; set; }
        [Display(Name = "Sender")]
        public string SenderUserName { get; set; }
        public string ReceiverUserId { get; set; }
        [Display(Name = "Receiver")]
        public string ReceiverUserName { get; set; }
        [Display(Name = "Size in Kilobytes")]
        public int SharedFileSize { get; set; }
        public byte[] SharedFileEncryptionKey { get; set; }
        public byte[] SharedFileIV { get; set; }
    }
}