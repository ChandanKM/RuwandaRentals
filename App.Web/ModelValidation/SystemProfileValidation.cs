using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class SystemProfileValidation1 : AbstractValidator<SystemProfileViewModel>
    {
        public SystemProfileValidation1()
        {
            RuleFor(x => x.Adr1).NotEmpty().WithMessage("Address 1 can't be blank");
            RuleFor(x => x.Adr2).NotEmpty().WithMessage("Address 2 can't be blank");
            RuleFor(x => x.CIN_Number).NotEmpty().WithMessage("CIN Number can't be blank");
            //  RuleFor(x => x.City).NotEmpty().WithMessage("Code can't be blank");
            RuleFor(x => x.Company_Title).NotEmpty().WithMessage("Company Title can't be blank");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Id can't be blank");
            RuleFor(x => x.Email).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid Contact Email Address");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location can't be blank");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("Mobile No. can't be blank");
            RuleFor(x => x.Mobile).Matches("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}").WithMessage("Mobile not valid");
            RuleFor(x => x.Owned_By).NotEmpty().WithMessage("Owned By can't be blank");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone No. can't be blank");
          
            RuleFor(x => x.Sms_Url).NotEmpty().WithMessage("Sms_Url can't be blank");
            RuleFor(x => x.Tin_id).NotEmpty().WithMessage("Tin Id can't be blank");
            RuleFor(x => x.User_Id).NotEmpty().WithMessage("User Name can't be blank");
            RuleFor(x => x.SetupEmail).NotEmpty().WithMessage("Setup Email Id can't be blank");
            RuleFor(x => x.SetupEmail).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid Contact Email Address");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can't be blank");
            RuleFor(x => x.SMTP).NotEmpty().WithMessage("SMTP Port Url can't be blank");
        }
    }
    public class SystemProfileValidation : AbstractValidator<SystemProfileViewModel>
    {
        public SystemProfileValidation()
        {
            RuleFor(x => x.Adr1).NotEmpty().WithMessage("Address 1 can't be blank");
            RuleFor(x => x.Adr2).NotEmpty().WithMessage("Address 2 can't be blank");
            RuleFor(x => x.CIN_Number).NotEmpty().WithMessage("CIN Number can't be blank");
            //  RuleFor(x => x.City).NotEmpty().WithMessage("Code can't be blank");
            RuleFor(x => x.Company_Title).NotEmpty().WithMessage("Company Title can't be blank");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Id can't be blank");
            RuleFor(x => x.Email).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid Contact Email Address");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location can't be blank");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("Mobile No. can't be blank");
            RuleFor(x => x.Mobile).Matches("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}").WithMessage("Mobile not valid");
            RuleFor(x => x.Owned_By).NotEmpty().WithMessage("Owned By can't be blank");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone No. can't be blank");

            RuleFor(x => x.Sms_Url).NotEmpty().WithMessage("Sms_Url can't be blank");
            RuleFor(x => x.Tin_id).NotEmpty().WithMessage("Tin Id can't be blank");
            RuleFor(x => x.User_Id).NotEmpty().WithMessage("User Name can't be blank");

        }
    }
}