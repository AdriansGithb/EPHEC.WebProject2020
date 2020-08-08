using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Entities
{
    public class EstablishmentsAddresses
    {
        [Key]
        public int EstablishmentId { get; set; }
        public Establishments Establishment { get; set; }

        [Required]
        [MaxLength (100)]
        public string Country { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string Street { get; set; }
        [Required]
        [MaxLength(20)]
        public string HouseNumber { get; set; }
        [MaxLength(20)]
        public string BoxNumber { get; set; }
    }
}
