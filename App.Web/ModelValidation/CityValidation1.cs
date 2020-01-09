using System;
using App.Web.ViewModels;
using FluentValidation;
namespace App.Web.ModelValidation
{
    public class CityValidation1:AbstractValidator<CityViewModel>
    {
        public CityValidation1()
        {
            RuleFor(x => x.City_Name).NotEmpty().WithMessage("City Name cannot be blank");
        }
    }
}