using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class GenderTypesModel
    {
        public GenderTypesId Id { get; set; }
        public string Name { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }
    }

    public enum GenderTypesId : int
    {
        Male = 0,
        Female = 1,
        Non_Binary = 2
    }
}
