using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Entities
{
    public class EstablishmentsPictures
    {
        public string Id { get; set; }
        
        public int EstablishmentId { get; set; }
        public Establishments Establishment { get; set; }

        [Required]
        public byte[] Picture { get; set; }
        public bool IsLogo { get; set; }
    }
}
