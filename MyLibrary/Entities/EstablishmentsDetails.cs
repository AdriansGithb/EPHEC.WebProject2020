using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Entities
{
    public class EstablishmentsDetails
    {
        [Key]
        public int EstablishmentId { get; set; }
        public Establishments Establishment { get; set; }

        [MaxLength(25)]
        public string Phone { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        [MaxLength(512)]
        public string ShortUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }
    }
}
