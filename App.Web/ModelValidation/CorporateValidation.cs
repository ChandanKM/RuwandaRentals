using App.Web.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.ModelValidation
{
    public class CorporateValidation : AbstractValidator<CorporateViewModel>
    {

    }

    public class CorporateLoginValidation : AbstractValidator<CorporateLoginViewModel>
    {

    }

    public class CorporateWebLoginValidation : AbstractValidator<CorporateWebLoginViewModel>
    {
        public CorporateWebLoginValidation()
        {

            RuleFor(x => x.Corp_mailid).NotNull().WithMessage("Email cannot be blank");
            RuleFor(x => x.Corp_mailid).EmailAddress().WithMessage("Not Valid Email Addresss");
            RuleFor(x => x.Corp_Pswd).NotNull().WithMessage("Password cannot be blank");

        }
    }
}