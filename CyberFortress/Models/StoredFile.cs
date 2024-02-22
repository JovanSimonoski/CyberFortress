using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberFortress.Models
{
    public class StoredFile
    {
        public int Id { get; set; }
        [Display(Name = "File")]
        [Required]
        public string StoredFileName { get; set; }
        [Required]
        public string StoredFilePath { get; set; }
        [Required]
        public string UserId { get; set; }
        [Display(Name = "Size in Kilobytes")]
        public int StoredFileSize { get; set; }
        public byte[] StoredFileEncryptionKey { get; set; }
        public byte[] StoredFileIV { get; set; }
    }
}