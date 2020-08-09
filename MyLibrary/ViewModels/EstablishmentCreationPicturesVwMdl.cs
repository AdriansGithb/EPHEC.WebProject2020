using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyLibrary.ViewModels
{
    public class EstablishmentCreationPicturesVwMdl
    {
        public IFormFile PictureFile { get; set; }
        public bool IsLogo { get; set; }

    }
}
