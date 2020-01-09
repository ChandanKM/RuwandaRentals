using System;
using App.Web.ViewModels;
using FluentValidation;


namespace App.Web.ModelValidation
{
    public class RoomValidation:AbstractValidator<RoomViewModel>
    {
         public RoomValidation()
        {
          
            //RuleFor(x => x.Room_Agreed_Availability).NotEmpty().WithMessage("Room offered to lmk cannot be blank");
            //RuleFor(x => x.Room_Lmk_Rate).NotEmpty().WithMessage("LMK rate cannot be blank");
            RuleFor(x => x.Room_Name).NotEmpty().WithMessage("Room Type can not be empty");
            //RuleFor(x => x.Room_Standard_rate).NotEmpty().WithMessage("Rack pricing cannot be blank");
            RuleFor(x => x.Room_Checkin).NotEmpty().WithMessage("Check In cannot be blank");
            RuleFor(x => x.Room_Checkout).NotEmpty().WithMessage("Check Out cannot be blank");
            //RuleFor(x => x.Room_Grace_time).NotEmpty().WithMessage("Grace time cannot be blank");
            RuleFor(x => x.Room_Overview).NotEmpty().WithMessage("Room Description cannot be blank");
           
           //RuleFor(x => x.Type_Id).NotEqual(0).WithMessage("Please select a RoomType!");

        }
    }
}