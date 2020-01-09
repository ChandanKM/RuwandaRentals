using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class LocationValidation: AbstractValidator<LocationViewModel>
    {
        public LocationValidation()
        {
            RuleFor(x => x.Location_desc).NotEmpty().WithMessage("Location cannot be blank");
        }
    }
}