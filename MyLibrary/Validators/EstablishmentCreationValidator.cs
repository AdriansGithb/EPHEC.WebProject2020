using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using MyLibrary.Constants;
using MyLibrary.Entities;
using MyLibrary.Helpers;
using MyLibrary.ViewModels;

namespace MyLibrary.Validators
{
    public class EstablishmentCreationValidator : AbstractValidator<EstablishmentCreationVwMdl>
    {
        public EstablishmentCreationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Establishment Name")
                .MaximumLength(50).WithMessage("Name field" + MyValidatorsConstants.HasMaxChar + "50")
                .Matches("^[- a-zA-Z0-9]+$").WithMessage("Name : " + MyValidatorsConstants.NoSpecialChar);

            RuleFor(x => x.VatNum)
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Vat Number")
                .MustAsync(async (vatNumber, cancellation) =>
                {
                    bool isValid = await VatLayerHelper.IsValidVatNumber(vatNumber);
                    return isValid;
                } ).WithMessage("VAT Number must be a valid vat number, with a valid vat format");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Email")
                .EmailAddress().WithMessage("Professional email field should be a valid email address (like establishmentpro@example.com)")
                .MaximumLength(255).WithMessage("Professional email field" + MyValidatorsConstants.HasMaxChar + "255");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description field" + MyValidatorsConstants.HasMaxChar + "2000");

            RuleFor(x => x.TypeId)
                .IsInEnum().WithMessage("Selected establishment type must be in GenderType enum");


            RuleFor(x => x.Details.Phone)
                .MaximumLength(25).WithMessage("Phone Number" + MyValidatorsConstants.HasMaxChar + "25")
                .Must(IsValidPhoneNum).WithMessage("Phone Number must be a valid fix or mobile phone number");

            RuleFor(x => x.Details.Email)
                .EmailAddress().WithMessage("Public email field should be a valid email address (like establishmentcontact@example.com)")
                .MaximumLength(255).WithMessage("Public email field" + MyValidatorsConstants.HasMaxChar + "255");

            RuleFor(x => x.Details.WebsiteUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Details.WebsiteUrl))
                .WithMessage("This is not a url. Please correct");

            RuleFor(x => x.Details.InstagramUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Details.InstagramUrl))
                .WithMessage("This is not a url. Please correct");

            RuleFor(x => x.Details.FacebookUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Details.FacebookUrl))
                .WithMessage("This is not a url. Please correct");

            RuleFor(x => x.Details.LinkedInUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Details.LinkedInUrl))
                .WithMessage("This is not a url. Please correct");

            RuleFor(x => x.Address.Country)
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Country");

            RuleFor(x => x.Address.City)
                .MaximumLength(100).WithMessage("City field "+MyValidatorsConstants.HasMaxChar+"100")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "City");

            RuleFor(x => x.Address.ZipCode)
                .MaximumLength(20).WithMessage("Zip Code field " + MyValidatorsConstants.HasMaxChar + "20")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Zip Code");
            
            RuleFor(x => x.Address.Street)
                .MaximumLength(100).WithMessage("Street field " + MyValidatorsConstants.HasMaxChar + "100")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Street");
            
            RuleFor(x => x.Address.HouseNumber)
                .MaximumLength(20).WithMessage("House number " + MyValidatorsConstants.HasMaxChar + "20")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "House number")
                .Matches("^[0-9]+$").WithMessage("House number must contains digits only. Add other characters into the Box field");

            RuleFor(x => x.Address.BoxNumber)
                .MaximumLength(20).WithMessage("Box number " + MyValidatorsConstants.HasMaxChar + "20");

            RuleForEach(x => x.OpeningTimes)
                .Must(IsValidOpeningHours).WithMessage("Opening/closing hours are empty for this open day. Please correct.");

            RuleFor(x => x.Logo.IsLogo)
                .Must(y => y == true).WithMessage("IsLogo field must be true, please contact an admin");

            RuleFor(x => x.Logo.PictureFile)
                .Must(IsValidLogoSize).WithMessage("Logo file maximum size is 1 Mb")
                .Must(IsValidPictureFormat)
                .WithMessage("Logo file must be a valid picture format : .jpg,.jpeg, .gif, .png, .pdf,.ico");

            RuleForEach(x => x.Pictures)
                .Must(HasValidPictureObjectSize).WithMessage("Picture file maximum size is 3 Mb")
                .Must(HasValidPictureObjectFormat)
                .WithMessage("Picture file must be a valid picture format : .jpg,.jpeg, .gif, .png, .pdf,.ico");



        }

        private bool IsValidPhoneNum(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return true;
            else return LibPhoneNumber.IsValidPhoneNumber(phoneNumber, false);
        }

        private bool IsValidOpeningHours(EstablishmentsOpeningTimes openingTime)
        {
            bool isValid = true;
            if (openingTime.IsOpen) 
            {
                if (!openingTime.OpeningHour.HasValue || !openingTime.ClosingHour.HasValue)
                    isValid = false;
            }
            return isValid;
        }

        private bool IsValidPictureFormat(IFormFile picture)
        {
            string[] allowedFileExtensions = new string[] { ".jpg",".jpeg", ".gif", ".png", ".pdf",".ico" };

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

        private bool HasValidPictureObjectSize(EstablishmentCreationPicturesVwMdl oPic)
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

        private bool HasValidPictureObjectFormat(EstablishmentCreationPicturesVwMdl oPic)
        {
            return IsValidPictureFormat(oPic.PictureFile);
        }

    }
}

