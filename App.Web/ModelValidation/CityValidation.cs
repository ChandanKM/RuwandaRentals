using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class CityValidation  : AbstractValidator<CityMasterViewModel>
    {
        public CityValidation()
        {
            RuleFor(x => x.City_Id).NotEmpty().WithMessage("City Name cannot be blank");
            RuleFor(x => x.location_Id).NotEmpty().WithMessage("Location cannot be blank");
            RuleFor(x => x.Pincode_Id).NotNull().WithMessage("Pincode cannot be blank");
          
        }

      
    }
}