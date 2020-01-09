using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class RoomTypeValidation : AbstractValidator<RoomTypeViewModel>
    {
        public RoomTypeValidation()
        {
            RuleFor(x => x.Room_Name).NotEmpty().WithMessage("Room Name can't be blank");
            RuleFor(x => x.Room_Descr).NotEmpty().WithMessage("Room Description can't be blank");
        }

    }
}