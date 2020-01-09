using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class SubscribeValidation  : AbstractValidator<SubscribeViewModel>
    {
        public SubscribeValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Id can't be blank");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email Id Is Not Valid");
        }
    }
}