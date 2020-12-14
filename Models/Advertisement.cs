using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DOTNET_lab4.Models
{
    public class Advertisement
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [DisplayName("File Name")]
        public string FileName { get; set; }
        [Required]
        [Url]
        public string Url { get; set; }
    }
}
