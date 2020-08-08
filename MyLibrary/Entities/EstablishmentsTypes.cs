using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Entities
{
    public class EstablishmentsTypes
    {
        public EstablishmentsTypesId Id { get; set; }
        public string Name { get; set; }

        public List<Establishments> Establishments { get; set; }
    }

    public enum EstablishmentsTypesId : int
    {
        Bar = 0,
        NightClub = 1,

        [Display(Name = "Concert Hall")]
        ConcertHall = 2,

        [Display(Name = "Students Association")] 
        StudentsAssociation = 3
    }


}
