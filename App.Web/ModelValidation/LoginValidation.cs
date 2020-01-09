using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class LoginValidation  : AbstractValidator<LoginViewModel>
    {
        public LoginValidation()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id/Email Id can't be blank");
           // RuleFor(x => x.UserId).EmailAddress().WithMessage("Email Id Is Not Valid");
            RuleFor(x => x.Pswd).NotEmpty().WithMessage("Password can't be blank");
        }     
    }


    public class ResetPasswordValidation : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordValidation()
        {

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be blank");
            RuleFor(x => x.Password).Length(6,12).WithMessage("Password Should be 6 character long.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password cannot be blank");
            RuleFor(x => x.ConfirmPassword).Equal(y => y.Password).WithMessage("New Password and Confirm Password Should be same.");
        }
    }
}