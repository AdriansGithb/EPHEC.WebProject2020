using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLibrary.Entities;

namespace MyLibrary.ViewModels
{
    public class UserAccountEditionVwMdl
    {        
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPassConfirmation { get; set; }
        public ApplicationUser UserDetails { get; set; }

    }
}
