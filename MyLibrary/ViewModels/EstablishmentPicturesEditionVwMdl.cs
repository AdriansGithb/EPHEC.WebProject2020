using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyLibrary.ViewModels
{
    public class EstablishmentPicturesEditionVwMdl
    {
        public int EstabId { get; set; }
        public string EstabName { get; set; }
        public string CurrentLogoId { get; set; }
        public List<string> CurrentPicturesId { get; set; }
        public List<PicturesEditionVwMdl> NewPictures { get; set; }
    }

    public class PicturesEditionVwMdl
    {
        public IFormFile PictureFile { get; set; }
        public byte[] PictureAsArray { get; set; }
        public bool IsLogo { get; set; }
    }
}
