using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyLibrary.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public bool IsProfessional { get; set; }
        public bool IsAdmin { get; set; }

        public GenderTypesId GenderType_Id { get; set; }
        public GenderTypes GenderType { get; set; }

        public List<Establishments> Establishments { get; set; }

    }
}
