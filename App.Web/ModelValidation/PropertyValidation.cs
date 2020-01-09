using App.Web.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.ModelValidation
{
    public class PropertyValidation : AbstractValidator<PropertyViewModel>
    {
        public PropertyValidation()
        {

            RuleFor(x => x.Prop_Name).NotEmpty().WithMessage("Property Name can not be blank");
            RuleFor(x => x.Prop_Cin_No).NotNull().WithMessage("CIN can not be blank");
            RuleFor(x => x.Prop_Addr1).NotNull().WithMessage("Address can not be empty");
            RuleFor(x => x.Prop_Overview).NotEmpty().WithMessage("Overview can not be empty");
            RuleFor(x => x.Prop_Star_Rating).NotEqual("not valid").WithMessage("Please check hotel rating!");
            RuleFor(x => x.Prop_Addr2).NotNull().WithMessage("Front desk phone number required");
            RuleFor(x => x.Prop_Addr2).Matches("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}").WithMessage("Front Desk Number not valid");
            RuleFor(x => x.State_Name).NotEmpty().WithMessage("State Name required");
            RuleFor(x => x.City_Name).NotEmpty().WithMessage("City Name required");
            RuleFor(x => x.Location_Name).NotEmpty().WithMessage("Location Name required");
            RuleFor(x => x.Location_Name).Matches("[\\w\\s]").WithMessage("Special symbols are not allowed in Location Field");
            //RuleFor(x => x.City_Id).NotNull().WithMessage("Please enter a city");
            //RuleFor(x => x.City_Id).NotEqual(0).WithMessage("Please enter a city");
            //RuleFor(x => x.City_Area).Matches(",").WithMessage("City Should have a Comma,Eg.HSR Layout,Bangalore");
            //RuleFor(x => x.City_Area).NotEmpty().WithMessage("Please select City");

            // RuleFor(x => x.Image_dir).NotEmpty().WithMessage("Image can not be blank");
            RuleFor(x => x.Prop_GPS_Pos).NotEmpty().WithMessage("GPS position can not be blank");
            RuleFor(x => x.Prop_Type).NotEmpty().WithMessage("Property Type  can not be blank");
            RuleFor(x => x.Prop_Pricing_MailId).NotEmpty().WithMessage("Please enter booking email id");
            RuleFor(x => x.Prop_Pricing_MailId).EmailAddress().WithMessage("Pricing email id is not valid");
            RuleFor(x => x.Prop_Pricing_MailId).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid Pricing Email Address");
            RuleFor(x => x.Prop_Pricing_Mob).NotEmpty().WithMessage("Please enter pricing mobile number");
            RuleFor(x => x.Prop_Pricing_Mob).Matches("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}").WithMessage("Pricing mobile number not valid");
            RuleFor(x => x.Prop_Star_Rating).NotNull().WithMessage("Please enter hotel star rating");

            RuleFor(x => x.Prop_Booking_MailId).NotEmpty().WithMessage("Please enter booking emailid");
            RuleFor(x => x.Prop_Booking_MailId).EmailAddress().WithMessage("Booking email id is not valid");
            RuleFor(x => x.Prop_Booking_MailId).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid Booking Email Address");
            RuleFor(x => x.Prop_Booking_Mob).NotEmpty().WithMessage("Please enter booking mobile number");
            RuleFor(x => x.Prop_Booking_Mob).Matches("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}").WithMessage("Booking mobile number not valid");

            RuleFor(x => x.Prop_Inventory_MailId).NotEmpty().WithMessage("Please enter inventory emailid");
            RuleFor(x => x.Prop_Inventory_MailId).EmailAddress().WithMessage("Inventory email id is not valid");
            RuleFor(x => x.Prop_Inventory_MailId).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid Inventory Email Address");
            RuleFor(x => x.Prop_Inventory_Mob).NotEmpty().WithMessage("Please enter inventory mobile number");
            RuleFor(x => x.Prop_Inventory_Mob).Matches("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}").WithMessage("Inventory mobile number not valid");
            //RuleFor(x => x.City_Id).NotEqual(0).WithMessage("Please select a City!");
            //RuleFor(x => x.Prop_Booking_MailId).NotEqual(y => y.Prop_Pricing_MailId).WithMessage("Booking Email Id and Pricing Email Id should not be the same!");
            //RuleFor(x => x.Prop_Booking_Mob).NotEqual(y => y.Prop_Pricing_Mob).WithMessage("Booking Mobile and Pricing Mobile should not be the same!");

        }
    }
    public class PropertyPolicyValidation : AbstractValidator<PropertyViewModel>
    {
        public PropertyPolicyValidation()
        {

            RuleFor(x => x.Policy_Name).NotEmpty().WithMessage("Policy Name can not be blank");

            RuleFor(x => x.Policy_descr).NotEmpty().WithMessage("Policy Description can not be blank");

        }
    }
    public class PropertyValidationBanck : AbstractValidator<PropertyViewModel>
    {
        public PropertyValidationBanck()
        {

            RuleFor(x => x.Bank_Name).NotEmpty().WithMessage("Bank Name can not be blank");
            RuleFor(x => x.Bank_Accnt_Name).NotEmpty().WithMessage("Account Name can not be blank");
            RuleFor(x => x.Bank_Accnt_No).NotEmpty().WithMessage("Account Number  can not be blank");
            RuleFor(x => x.Bank_Branch_Code).NotEmpty().WithMessage("Branch Code  can not be blank");
            RuleFor(x => x.Bank_Branch_Name).NotEmpty().WithMessage("Branch Name  can not be blank");
            RuleFor(x => x.Bank_IFC_code).NotEmpty().WithMessage("IFSC Code  can not be blank");
            RuleFor(x => x.City_Id).NotNull().WithMessage("Please select City!");
            RuleFor(x => x.City_Id).NotEqual(0).WithMessage("Please enter a city");
            //RuleFor(x => x.City_Area).Matches(",").WithMessage("City Should have a Comma,Eg.HSR Layout,Bangalore");
            //RuleFor(x => x.City_Area).NotEmpty().WithMessage("Please select Location");

        }
    }
}