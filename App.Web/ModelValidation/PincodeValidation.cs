using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class PincodeValidation : AbstractValidator<PincodeViewModel>
    {
        public PincodeValidation()
        {
            RuleFor(x => x.Pincode).NotEmpty().WithMessage("Pincode cannot be blank");
        }
    }
}