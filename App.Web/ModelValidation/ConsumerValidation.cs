using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class ConsumerValidation : AbstractValidator<ConsumerViewModel>
    {
        public ConsumerValidation()
        {
            RuleFor(x => x.Cons_First_Name).NotEmpty().WithMessage("First Name cannot be blank");
            RuleFor(x => x.Cons_Last_Name).NotEmpty().WithMessage("Last Name cannot be blank");
            RuleFor(x => x.Cons_Mobile).NotNull().WithMessage("Mobile cannot be blank");
            RuleFor(x => x.Cons_mailid).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid  Email Address");
            RuleFor(x => x.Cons_mailid).NotNull().WithMessage("Email cannot be blank");
            RuleFor(x => x.Cons_Pswd).NotNull().WithMessage("Password cannot be blank");

        }


    }

    public class ConsumerMandetValidation : AbstractValidator<ConsumerMandetViewModel>
    {
        public ConsumerMandetValidation()
        {
            RuleFor(x => x.Cons_First_Name).NotEmpty().WithMessage("First Name cannot be blank");
            RuleFor(x => x.Cons_Last_Name).NotEmpty().WithMessage("Last Name cannot be blank");
            RuleFor(x => x.Cons_Mobile).NotNull().WithMessage("Mobile cannot be blank");
            RuleFor(x => x.Cons_mailid).NotNull().WithMessage("Email cannot be blank");
            //RuleFor(x => x.Cons_mailid).EmailAddress().WithMessage("Not a Valid Email");
            RuleFor(x => x.Cons_mailid).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid  Email Address");
            RuleFor(x => x.Cons_Pswd).NotEmpty().WithMessage("Password cannot be blank");
       //     RuleFor(x => x.Cons_Pswd).Length(6, 12).WithMessage("Password Should be 6 character long.");

        }


    }

    public class ConsumerLoginValidation : AbstractValidator<ConsumerLoginViewModel>
    {
        public ConsumerLoginValidation()
        {

            RuleFor(x => x.Cons_mailid).NotNull().WithMessage("Email cannot be blank");
            RuleFor(x => x.Cons_Pswd).NotNull().WithMessage("Password cannot be blank");

        }


    }

    public class ConsumerDetailsValidation : AbstractValidator<ConsumerDetailsViewModel>
    {
        public ConsumerDetailsValidation()
        {

            RuleFor(x => x.Cons_Id).NotNull().WithMessage("Id cannot be blank");


        }


    }
    public class ConsumerForgotpwdValidation : AbstractValidator<ConsumerForgotpwdViewModel>
    {
        public ConsumerForgotpwdValidation()
        {

            RuleFor(x => x.Cons_mailid).NotNull().WithMessage("Email cannot be blank");
            RuleFor(x => x.Cons_mailid).EmailAddress().WithMessage("Not a Valid Email");

        }


    }


    #region WebApi
    public class ConsumerSignUpValidation : AbstractValidator<ConsumerMandetViewModel>
    {
        public ConsumerSignUpValidation()
        {
            RuleFor(x => x.Cons_First_Name).NotEmpty().WithMessage("First Name cannot be blank");
            RuleFor(x => x.Cons_Last_Name).NotEmpty().WithMessage("Last Name cannot be blank");
            RuleFor(x => x.Cons_Mobile).NotNull().WithMessage("Mobile cannot be blank");
            RuleFor(x => x.Cons_mailid).NotNull().WithMessage("Email cannot be blank");
            //RuleFor(x => x.Cons_mailid).EmailAddress().WithMessage("Not a Valid Email");
            RuleFor(x => x.Cons_mailid).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid  Email Address");
            RuleFor(x => x.Cons_Pswd).NotEmpty().WithMessage("Password cannot be blank");
            RuleFor(x => x.Cons_Pswd).Length(6, 12).WithMessage("Password Should be 6 character long.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password cannot be blank");
            RuleFor(x => x.ConfirmPassword).Equal(y => y.Cons_Pswd).WithMessage("New Password and Confirm Password Should be same.");
        }
    }

    public class ConsumerProfileValidation : AbstractValidator<ConsumerFormViewModel>
    {
        public ConsumerProfileValidation()
        {
            RuleFor(x => x.Cons_First_Name).NotEmpty().WithMessage("First Name cannot be blank");
            RuleFor(x => x.Cons_Last_Name).NotEmpty().WithMessage("Last Name cannot be blank");
            RuleFor(x => x.Cons_Mobile).NotNull().WithMessage("Mobile cannot be blank");
            RuleFor(x => x.Cons_mailid).Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$").WithMessage("Please enter valid  Email Address");
            RuleFor(x => x.Cons_mailid).NotNull().WithMessage("Email cannot be blank");
        }


    }

    public class ConsumerChagePasswordValidation : AbstractValidator<ConsumerChangePasswordViewModel>
    {
        public ConsumerChagePasswordValidation()
        {
            RuleFor(x => x.Curnt_Pswd).NotEmpty().WithMessage("Current Password cannot be blank");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New Password cannot be blank");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password cannot be blank");
            RuleFor(x => x.ConfirmPassword).Equal(y => y.NewPassword).WithMessage("New Password and Confirm Password Should be same.");
            
        }
    }
    public class ConsumerChagePasswordValidation1 : AbstractValidator<ConsumerViewModel>
    {
        public ConsumerChagePasswordValidation1()
        {
            RuleFor(x => x.Cons_Pswd).NotEmpty().WithMessage("Current Password cannot be blank");

        }
    }

    public class PrebookingValidation : AbstractValidator<PrebookingViewModel>
    {
        public PrebookingValidation()
        {
            RuleFor(x => x.Cons_Id).NotEmpty().WithMessage("*");
            RuleFor(x => x.GuestName).NotEmpty().WithMessage("Occupant detail can't be blank");
            //RuleFor(x => x.Invce_note).NotEmpty().WithMessage("Invoice Note cannot be blank");
        }
    }

    public class ConsumerUpdateProfileValidation : AbstractValidator<ConsumerFormViewModel>
    {
        public ConsumerUpdateProfileValidation()
        {
            RuleFor(x => x.Cons_Dob).NotEmpty().WithMessage("Date of birth can't be blank");
            RuleFor(x => x.Cons_Mobile).NotEmpty().WithMessage("Mobile Number can't be blank");
            RuleFor(x => x.Cons_Mobile).Length(10).WithMessage("Mobile Number Not Valid");

            RuleFor(x => x.Cons_City).NotEmpty().WithMessage("City can't be blank");
            RuleFor(x => x.Cons_Addr1).NotEmpty().WithMessage("Address can't be blank");
            RuleFor(x => x.Cons_Area).NotEmpty().WithMessage("Please Select the City & Location");
        }
    }
    public class SearchHotelsValidation : AbstractValidator<PropertyViewModel>
    {
        public SearchHotelsValidation()
        {
            RuleFor(x => x.CityMasterId).NotEmpty().WithMessage("Please Select the City & Location");
            RuleFor(x => x.No_Of_Rooms).NotEmpty().WithMessage("Please Select the No. of Room");
            //RuleFor(x => x.Room_Checkin).Da.WithMessage("Mobile Number Not Valid");
            //RuleFor(x => x.Room_Checkout).NotEmpty().WithMessage("City can't be blank");
        }
    }
    public class FeedBackValidation : AbstractValidator<FeedBackViewModel>
    {
        public FeedBackValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter the Name");
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Please enter the email address");
            RuleFor(x => x.EmailAddress).EmailAddress().WithMessage("Not a valid email addresss");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("Mobile Number can't be blank");
            RuleFor(x => x.Mobile).Length(10).WithMessage("Mobile Number Not Valid");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message Can't be blank");
        }
    }

    public class AddFeedBackValidation : AbstractValidator<FeedBackViewModel>
    {
        public AddFeedBackValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter the name");
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Please enter the email address");
            RuleFor(x => x.EmailAddress).EmailAddress().WithMessage("Not a valid email addresss");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message Can't be blank");
        }
    }

    public class ConsumerWebLoginValidation : AbstractValidator<ConsumerWebLoginViewModel>
    {
        public ConsumerWebLoginValidation()
        {

            RuleFor(x => x.Cons_mailid).NotNull().WithMessage("Email cannot be blank");
            RuleFor(x => x.Cons_mailid).EmailAddress().WithMessage("Not Valid Email Addresss");
            RuleFor(x => x.Cons_Pswd).NotNull().WithMessage("Password cannot be blank");

        }
    }
    #endregion
}