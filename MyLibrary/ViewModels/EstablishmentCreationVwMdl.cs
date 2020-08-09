using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLibrary.Entities;

namespace MyLibrary.ViewModels
{
    public class EstablishmentCreationVwMdl
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(25)]
        public string VatNum { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public bool IsValidated { get; set; }

        public string ManagerId { get; set; }

        public EstablishmentsTypesId TypeId { get; set; }

        public EstablishmentsDetails Details { get; set; }
        public EstablishmentsAddresses Address { get; set; }

        public List<EstablishmentsOpeningTimes> OpeningTimes { get; set; }
        public EstablishmentsPictures Logo { get; set; }

        public List<EstablishmentsPictures> Pictures { get; set; }







    }
}
