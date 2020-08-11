using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyLibrary.ViewModels
{
    public class EstablishmentShortVwMdl
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EstabType { get; set; }
        public string ManagerId { get; set; }
        public string ManagerUserName { get; set; }
        public bool IsValidated { get; set; }

    }
}
