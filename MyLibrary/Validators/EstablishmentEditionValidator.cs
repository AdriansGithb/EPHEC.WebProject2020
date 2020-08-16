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
using MyLibrary.DTOs;
using MyLibrary.Entities;
using MyLibrary.Helpers;
using MyLibrary.ViewModels;

namespace MyLibrary.Validators
{
    public class EstablishmentEditionValidator : AbstractValidator<EstablishmentEditionVwMdl>
    {
        public EstablishmentEditionValidator()
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
            }).WithMessage("VAT Number must be a valid vat number, with a valid vat format");

            RuleFor(x => x.EmailPro)
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Email")
                .EmailAddress().WithMessage("Professional email field should be a valid email address (like establishmentpro@example.com)")
                .MaximumLength(255).WithMessage("Professional email field" + MyValidatorsConstants.HasMaxChar + "255");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description field" + MyValidatorsConstants.HasMaxChar + "2000");

            RuleFor(x => x.TypeId)
                .IsInEnum().WithMessage("Selected establishment type must be in GenderType enum");


            RuleFor(x => x.Phone)
                .MaximumLength(25).WithMessage("Phone Number" + MyValidatorsConstants.HasMaxChar + "25")
                .Must(IsValidPhoneNum).WithMessage("Phone Number must be a valid fix or mobile phone number");

            RuleFor(x => x.PublicEmail)
                .EmailAddress().WithMessage("Public email field should be a valid email address (like establishmentcontact@example.com)")
                .MaximumLength(255).WithMessage("Public email field" + MyValidatorsConstants.HasMaxChar + "255");

            RuleFor(x => x.WebsiteUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.WebsiteUrl))
                .WithMessage("This is not a url. Please correct");

            RuleFor(x => x.InstagramUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.InstagramUrl))
                .WithMessage("This is not a url. Please correct");

            RuleFor(x => x.FacebookUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.FacebookUrl))
                .WithMessage("This is not a url. Please correct");

            RuleFor(x => x.LinkedInUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.LinkedInUrl))
                .WithMessage("This is not a url. Please correct");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Country");

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("City field " + MyValidatorsConstants.HasMaxChar + "100")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "City");

            RuleFor(x => x.ZipCode)
                .MaximumLength(20).WithMessage("Zip Code field " + MyValidatorsConstants.HasMaxChar + "20")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Zip Code");

            RuleFor(x => x.Street)
                .MaximumLength(100).WithMessage("Street field " + MyValidatorsConstants.HasMaxChar + "100")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Street");

            RuleFor(x => x.HouseNumber)
                .MaximumLength(20).WithMessage("House number " + MyValidatorsConstants.HasMaxChar + "20")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "House number")
                .Matches("^[0-9]+$").WithMessage("House number must contains digits only. Add other characters into the Box field");

            RuleFor(x => x.BoxNumber)
                .MaximumLength(20).WithMessage("Box number " + MyValidatorsConstants.HasMaxChar + "20");

            RuleForEach(x => x.OpeningTimesList)
                .Must(IsValidOpeningHours).WithMessage("Opening/closing hours are empty for this open day. Please correct.");

        }

        private bool IsValidPhoneNum(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return true;
            else return LibPhoneNumber.IsValidPhoneNumber(phoneNumber, false);
        }

        private bool IsValidOpeningHours(OpeningTimesDTO openingTime)
        {
            bool isValid = true;
            if (openingTime.IsOpen)
            {
                if (!openingTime.OpeningHour.HasValue || !openingTime.ClosingHour.HasValue)
                    isValid = false;
            }
            return isValid;
        }

    }
}
