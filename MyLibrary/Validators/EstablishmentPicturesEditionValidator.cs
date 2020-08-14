using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using MyLibrary.ViewModels;

namespace MyLibrary.Validators
{
    public class EstablishmentPicturesEditionValidator : AbstractValidator<EstablishmentPicturesEditionVwMdl>
    {
        public EstablishmentPicturesEditionValidator()
        {
            RuleFor(x => x.NewPictures.Find(p=>p.IsLogo==true).PictureFile)
                .Must(IsValidLogoSize).WithMessage("Logo file maximum size is 1 Mb")
                .Must(IsValidPictureFormat)
                .WithMessage("Logo file must be a valid picture format : .jpg,.jpeg, .gif, .png, .pdf,.ico");

            RuleForEach(x => x.NewPictures)
                .Must(HasValidPictureObjectSize).WithMessage("Picture file maximum size is 3 Mb")
                .Must(HasValidPictureObjectFormat)
                .WithMessage("Picture file must be a valid picture format : .jpg,.jpeg, .gif, .png, .pdf,.ico");

        }


        private bool IsValidPictureFormat(IFormFile picture)
        {
            string[] allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".pdf", ".ico" };

            if (picture == null)
                return true;
            else if (!allowedFileExtensions.Contains(Path.GetExtension(picture.FileName)))
            {
                return false;
            }
            else
                return true;
        }

        private bool IsValidLogoSize(IFormFile logo)
        {
            int maxContentLength = 1024 * 1024 * 1;


            if (logo == null)
                return true;
            else if (logo.Length > maxContentLength)
            {
                return false;
            }
            else
                return true;
        }

        private bool HasValidPictureObjectSize(PicturesEditionVwMdl oPic)
        {
            int maxContentLength = 1024 * 1024 * 3;

            if (oPic.PictureFile == null)
                return true;
            else if (oPic.PictureFile.Length > maxContentLength)
            {
                return false;
            }
            else
                return true;
        }

        private bool HasValidPictureObjectFormat(PicturesEditionVwMdl oPic)
        {
            return IsValidPictureFormat(oPic.PictureFile);
        }

    }

}


