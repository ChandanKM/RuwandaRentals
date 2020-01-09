using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class PromotionValidation  : AbstractValidator<PromotionViewModel>
    {
        public PromotionValidation()
        {
            RuleFor(x => x.Promo_descr).NotNull().WithMessage("Promo_descr Name cannot be blank");
            RuleFor(x => x.Promo_Code).NotNull().WithMessage("Promo_Code cannot be blank");
            RuleFor(x => x.Promo_Type).NotEmpty().WithMessage("Promo_Type cannot be blank");
            RuleFor(x => x.Prop_Value).NotNull().WithMessage("Prop_Value cannot be blank");
            RuleFor(x => x.Promo_Start).NotNull().WithMessage("Promo_Start cannot be blank");
            RuleFor(x => x.Promo_End).NotNull().WithMessage("Promo_End cannot be blank");
        }

      
    }
}