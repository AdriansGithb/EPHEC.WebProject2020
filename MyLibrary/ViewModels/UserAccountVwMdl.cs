using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.ViewModels
{
    public class UserAccountVwMdl
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }
        public bool IsProfessional { get; set; }
        public bool IsAdmin { get; set; }
        public string Gender { get; set; }

    }
}
