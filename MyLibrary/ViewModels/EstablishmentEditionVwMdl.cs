using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLibrary.DTOs;
using MyLibrary.Entities;

namespace MyLibrary.ViewModels
{
    public class EstablishmentEditionVwMdl
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VatNum { get; set; }
        public string EmailPro { get; set; }
        public string Description { get; set; }
        public bool IsValidated { get; set; }
        public EstablishmentsTypesId TypeId { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string BoxNumber { get; set; }



        public string Phone { get; set; }
        public string PublicEmail { get; set; }
        public string WebsiteUrl { get; set; }
        public string ShortUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }


        public List<OpeningTimesDTO> OpeningTimesList { get; set; }

    }
}
