using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Entities
{
    public class NewsPictures
    {
        [Key]
        public string NewsId { get; set; }
        public EstablishmentsNews News { get; set; }

        [Required]
        public byte[] Picture { get; set; }
    }
}
