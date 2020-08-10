using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Entities
{
    public class Establishments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(25)]
        public string VatNum { get; set; }
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public bool IsValidated { get; set; }

        [Required]
        public string ManagerId { get; set; }
        public ApplicationUser Manager { get; set; }

        public EstablishmentsTypesId TypeId { get; set; }
        public EstablishmentsTypes Type { get; set; }

        public EstablishmentsDetails Details { get; set; }
        public EstablishmentsAddresses Address { get; set; }

        public List<EstablishmentsOpeningTimes> OpeningTimes { get; set; }
        
        public List<EstablishmentsPictures> Pictures { get; set; }

        public List<EstablishmentsNews> News { get; set; }
    }
}
