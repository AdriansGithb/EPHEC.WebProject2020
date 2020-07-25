using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Entities
{
    public class GenderTypes
    {
        public GenderTypesId Id { get; set; }
        public string Name { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }
    }

    public enum GenderTypesId : int
    {
        Male = 0,
        Female = 1,
        [Display(Name = "Non Binary")]
        Non_Binary = 2
    }
}
