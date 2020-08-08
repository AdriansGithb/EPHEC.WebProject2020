using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Entities
{
    public class EstablishmentsOpeningTimes
    {
        public string Id { get; set; }

        public int EstablishmentId { get; set; }
        public Establishments Establishment { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public bool IsSpecialDay { get; set; }
        [DataType(DataType.Date)]
        public DateTime? SpecialDayDate  { get; set; }
        public bool IsOpen { get; set; }
        public DateTime? OpeningHour { get; set; }
        public DateTime? ClosingHour { get; set; }
    }
}
