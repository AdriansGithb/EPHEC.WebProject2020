using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MyLibrary.Constants;
using MyLibrary.Entities;
using MyLibrary.ViewModels;
using MyLibrary.Helpers;

namespace MyLibrary.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterVwMdl>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserDetails.Email)
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Email")
                .EmailAddress().WithMessage("Email field should be a valid email address (like user@example.com)")
                .MaximumLength(255).WithMessage("Email field" + MyValidatorsConstants.HasMaxChar + "255");

            RuleFor(x => x.Password)
                .Equal(x => x.PassConfirmation)
                .WithMessage("Password field must be equal to Password confirmation field")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Password");

            RuleFor(x => x.PassConfirmation)
                .Equal(x => x.Password)
                .WithMessage("Password field must be equal to Password confirmation field")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "Password Confirmation");
            
            RuleFor(x => x.UserDetails.LastName)
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "LastName")
                .MaximumLength(50).WithMessage("LastName field" + MyValidatorsConstants.HasMaxChar + "50")
                .Matches("^[- a-zA-Z]+$").WithMessage("LastName : " + MyValidatorsConstants.NoSpecialChar);

            RuleFor(x => x.UserDetails.FirstName)
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "FirstName")
                .MaximumLength(50).WithMessage("FirstName field" + MyValidatorsConstants.HasMaxChar + "50")
                .Matches("^[- a-zA-Z]+$").WithMessage("FirstName : " + MyValidatorsConstants.NoSpecialChar);

            RuleFor(x => x.UserDetails.BirthDate)
                .LessThan(DateTime.Today.Date).WithMessage("The Date of Birth can't be later or equal to today")
                .NotEmpty().WithMessage(MyValidatorsConstants.RequiredField + "BirthDate");

            RuleFor(x => x.UserDetails.GenderType_Id)
                .IsInEnum().WithMessage("Selected gender type must be in GenderType enum");

            RuleFor(x => x.UserDetails.PhoneNumber)
                .MaximumLength(25).WithMessage("Mobile Phone Number" + MyValidatorsConstants.HasMaxChar + "25")
                .Must(IsValidPhoneNum).WithMessage("Phone Number must be a valid mobile phone number");



        }

        private bool IsValidPhoneNum(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return true;
            else return LibPhoneNumber.IsValidPhoneNumber(phoneNumber, true);
        }

    }
}
