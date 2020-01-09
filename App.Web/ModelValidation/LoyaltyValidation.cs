using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class LoyaltyValidation : AbstractValidator<LoyaltyViewModel>
    {
        public LoyaltyValidation()
        {
            RuleFor(x => x.Loyal_Desc).NotNull().WithMessage("Location cannot be blank");
            RuleFor(x => x.Loyal_Start_On).NotNull().WithMessage("Loyal_Start_On cannot be blank");
            RuleFor(x => x.Loyal_End_On).NotNull().WithMessage("Loyal_End_On cannot be blank");
            RuleFor(x => x.Loyal_Set_Up).NotNull().WithMessage("Loyal_Set_Up cannot be blank");
            RuleFor(x => x.Loyal_Checked_By).NotNull().WithMessage("Loyal_Checked_By cannot be blank");
            RuleFor(x => x.Loyal_Approved_By).NotNull().WithMessage("Loyal_Approved_By cannot be blank");
            RuleFor(x => x.Loyal_Max_Allowed).NotEmpty().WithMessage("Loyal_Max_Allowed cannot be blank");
            RuleFor(x => x.Loyal_Min_redmpt).NotEmpty().WithMessage("Loyal_Min_redmpt cannot be blank");
            RuleFor(x => x.Loyal_Max_redmpt).NotEmpty().WithMessage("Loyal_Max_redmpt cannot be blank");
        }
    }
}