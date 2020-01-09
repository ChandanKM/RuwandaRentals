using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class FacilityValidation:AbstractValidator<FacilityViewModel>
    {
        public FacilityValidation()
        {
            //RuleFor(x => x.Facility_Id).NotEmpty().WithMessage("Facility Id cannot be blank");
            RuleFor(x => x.Facility_Name).NotEmpty().WithMessage("Facility Name cannot be blank");
            //RuleFor(x => x.Facility_Type).NotEmpty().WithMessage("Facility Type cannot be blank");
            RuleFor(x => x.Facility_descr).NotEmpty().WithMessage("Description cannot be blank");
            RuleFor(x => x.Facility_Image_dir).NotEmpty().WithMessage("Image is Mandatory");
           
        }
    }
}