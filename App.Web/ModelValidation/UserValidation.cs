using System;
using App.Web.ViewModels;
using FluentValidation;

namespace App.Web.ModelValidation
{
    public class UserValidation  : AbstractValidator<UserViewModel>
    {
        public UserValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name cannot be blank");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name cannot be blank");
            RuleFor(x => x.UserName).NotNull().WithMessage("User name cannot be blank");
            RuleFor(x => x.Password).NotNull().WithMessage("Password cannot be blank");
            RuleFor(x => x.UserName).Equal(x => x.FirstName).WithMessage("UserName should  be a an EmailId");
        }

      
    }
}