using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.DTOs
{
    public class AddressDTO
    {
        public int EstablishmentId { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentType { get; set; }



        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string BoxNumber { get; set; }

        public string OpenHour { get; set; }
        public string CloseHour { get; set; }
    }
}
