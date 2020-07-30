using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MyLibrary.Entities;

namespace MyLibrary.ViewModels
{
    public class RegisterVwMdl
    {
        public string Password { get; set; }
        public string PassConfirmation { get; set; }
        public ApplicationUser UserDetails { get; set; }


    }
}
