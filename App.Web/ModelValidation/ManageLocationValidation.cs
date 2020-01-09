using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class ManageLocationValidation : AbstractValidator<ManageLocationViewModel>
    {
        public ManageLocationValidation()
        {
            RuleFor(x => x.City).NotEmpty().WithMessage("City can't be blank");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location can't be blank");
            RuleFor(x => x.Pincode).NotEmpty().WithMessage("Pincode can't be blank");
            RuleFor(x => x.Pincode).Matches("0-9").WithMessage("only Numeric Key Allow");
            RuleFor(x => x.Pincode).Length(6).WithMessage("6-Digit Pincode Required");
            RuleFor(x => x.State).NotEmpty().WithMessage("State can't be blank");
        }

    }
}