using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class VendorValidation : AbstractValidator<VendorViewModel>
    {
        public VendorValidation()
        {
            RuleFor(x => x.Vndr_Name).NotEmpty().WithMessage("Name cannot be blank");
            RuleFor(x => x.Vndr_Cinno).NotEmpty().WithMessage("Cin No cannot be blank");
            //RuleFor(x => x.Vndr_Contact_Mobile).NotEmpty().WithMessage("Contact person mobile number can't be blank");
            RuleFor(x => x.Vndr_Addr1).NotEmpty().WithMessage("Address can not be empty");
            RuleFor(x => x.Vndr_Contact_person).NotEmpty().WithMessage("Admin contact person name required");

            RuleFor(x => x.Vndr_Contact_Email).NotEmpty().WithMessage("Admin contact Email  required");
            RuleFor(x => x.Vndr_Contact_Email).EmailAddress().WithMessage("Please enter valid Contact Email");
            RuleFor(x => x.Vndr_Contact_Email).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid Contact Email Address");
          
            RuleFor(x => x.Vndr_Contact_Mobile).NotEmpty().WithMessage("Contact mobile can not be empty");
            RuleFor(x => x.Vndr_Contact_Mobile).NotNull().Length(10).Matches("[0-9]").WithMessage("Please enter valid mobile number");
            RuleFor(x => x.Vndr_Contact_Mobile).Matches("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}").WithMessage("Vendor contact mobile not valid");
            //  RuleFor(x => x.Vndr_Contact_Mobile).NotNull().Length(10).Matches(".").WithMessage("Please enter valid mobile number");

            RuleFor(x => x.Vndr_Alternate_Email).NotEmpty().WithMessage("Alternate Email  required");
            RuleFor(x => x.Vndr_Alternate_Email).EmailAddress().WithMessage("Please enter valid Alternate Email");
            RuleFor(x => x.Vndr_Alternate_Email).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid Alternate Email Address");

            RuleFor(x => x.Vndr_Alternate_Mobile).NotEmpty().WithMessage("Alternate mobile can not be empty");
            RuleFor(x => x.Vndr_Alternate_Mobile).NotNull().Length(10).Matches("[0-9]").WithMessage("Please enter valid alternate mobile number");
            RuleFor(x => x.Vndr_Alternate_Mobile).Matches("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}").WithMessage("Vendor alternate mobile not valid");
            RuleFor(x => x.City_Area).NotNull().NotEmpty().WithMessage("Please choose a pincode");

            //    RuleFor(x => x.Vndr_Contact_person).NotEqual(y => y.Vndr_Alternate_person).WithMessage("Contact Person  and Pricing  Alternate Person should not be the same!");
         
           RuleFor(x => x.Vndr_Alternate_Mobile).NotEqual(y => y.Vndr_Alternate_Nos).WithMessage("Alternate Mobile and Alternate Phone should not be the same!");
           RuleFor(x => x.Vndr_Alternate_Email).NotEqual(y => y.Vndr_Contact_Email).WithMessage("Alternate Email and Contact Email should not be the same!");
            //    RuleFor(x => x.Vndr_Contact_Nos).NotEqual(y => y.Vndr_Alternate_Nos).WithMessage("Contact Phone and Alternate Phone should not be the same!");
            //    RuleFor(x => x.Vndr_Contact_Email).NotEqual(y => y.Vndr_Alternate_Email).WithMessage("Contact Email and Alternate Email should not be the same!");
        }

        private bool BeAValidMobile(string postcode)
        {
            return false;
            // custom postcode validating logic goes here
        }
    }

    public class VendorIdValidation : AbstractValidator<VendorIDViewModel>
    {
        public VendorIdValidation()
        {
            RuleFor(x => x.Vendor_ID).NotEmpty().WithMessage("Name cannot be blank");
        }
    }
}