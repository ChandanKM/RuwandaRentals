using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class UserProfileValidation : AbstractValidator<UserProfileViewModel>
    {
        public UserProfileValidation()
        {
            //RuleFor(x => x.Authority_Id).NotEqual(0).WithMessage("Please Select User Type");
            RuleFor(x => x.User_Name).NotEmpty().WithMessage("EmailId cannot be blank");
            RuleFor(x => x.User_Name).EmailAddress().WithMessage("Invalid Email Address");
            RuleFor(x => x.Firstname).NotEmpty().WithMessage("Firstname cannot be blank");
            RuleFor(x => x.Lastname).NotEmpty().WithMessage("Lastname cannot be blank");
            RuleFor(x => x.Pswd).NotEmpty().WithMessage("Password cannot be blank");
            RuleFor(x => x.Pswd).Length(6, 12).WithMessage("Password Should be 6 character long.");
            //RuleFor(x => x.Department).NotEmpty().WithMessage("Department cannot be blank");
        }
    }
    public class UserProfileValidationupdate : AbstractValidator<UserProfileViewModel>
    {
        public UserProfileValidationupdate()
        {
            //RuleFor(x => x.Authority_Id).NotEqual(0).WithMessage("Please Select User Type");
            RuleFor(x => x.User_Name).NotEmpty().WithMessage("User Name cannot be blank");
            RuleFor(x => x.User_Name).EmailAddress().WithMessage("Invalid Email Address");
            RuleFor(x => x.Firstname).NotEmpty().WithMessage("Firstname cannot be blank");
            RuleFor(x => x.Lastname).NotEmpty().WithMessage("Lastname cannot be blank");
         
            //RuleFor(x => x.Department).NotEmpty().WithMessage("Department cannot be blank");
        }
    }
    public class ParamValidation : AbstractValidator<ParamViewModel>
    {
        public ParamValidation()
        {
            RuleFor(x => x.Vparam_Code).NotEmpty().WithMessage("Param Code cannot be blank");
            RuleFor(x => x.Vparam_Descr).NotEmpty().WithMessage("Param Description cannot be blank");
            RuleFor(x => x.Vparam_Val).NotEmpty().WithMessage("Param Value cannot be blank");
        }
    }
}