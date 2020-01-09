using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class ForgotLinkValidation  : AbstractValidator<ForgotLinkViewModel>
    {
        public ForgotLinkValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Id can't be blank");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email Id Is Not Valid");
        }
    }
}