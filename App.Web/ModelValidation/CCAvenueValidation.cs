using App.Web.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.ModelValidation
{
    public class CCAvenueValidation: AbstractValidator<CCAvenueViewModel>
    {
        public CCAvenueValidation()
        {
            RuleFor(x => x.Cav_Name).NotEmpty().WithMessage("CC Avenue Name can't be blank");
            RuleFor(x => x.Cav_Descr).NotEmpty().WithMessage("CC Avenue Description can't be blank");
            RuleFor(x => x.Cav_Percent).NotEmpty().WithMessage("CC Avenue Percent can't be blank");
        }
    }
}
