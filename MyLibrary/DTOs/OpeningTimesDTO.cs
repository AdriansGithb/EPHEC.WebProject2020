using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.DTOs
{
    public class OpeningTimesDTO
    {
        public DayOfWeek DayOfWeek { get; set; }
        public bool IsOpen { get; set; }
        [DataType(DataType.Time), DisplayFormat(DataFormatString= "{0:t}") ]
        public DateTime? OpeningHour { get; set; }
        [DataType(DataType.Time), DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime? ClosingHour { get; set; }

    }
}
