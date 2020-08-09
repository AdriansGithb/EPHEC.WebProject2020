using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLibrary.Entities;
using System.Web;

namespace MyLibrary.ViewModels
{
    public class EstablishmentCreationVwMdl
    {
        public string Name { get; set; }
        public string VatNum { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool IsValidated { get; set; }

        public string ManagerId { get; set; }

        public EstablishmentsTypesId TypeId { get; set; }

        public EstablishmentsDetails Details { get; set; }
        public EstablishmentsAddresses Address { get; set; }

        public List<EstablishmentsOpeningTimes> OpeningTimes { get; set; }
        public EstablishmentCreationPicturesVwMdl Logo { get; set; }

        public List<EstablishmentCreationPicturesVwMdl> Pictures { get; set; }







    }
}
