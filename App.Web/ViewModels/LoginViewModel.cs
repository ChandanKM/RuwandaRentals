using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using App.Common;

namespace App.Web.ViewModels
{
    public class LoginViewModel : TransactionStatus
    {
        [Required(ErrorMessage = "Please Enter Email Id")]
        [EmailAddress(ErrorMessage = "Invalid Id")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Pswd { get; set; }
    }

    public class ForgotLinkViewModel : TransactionStatus
    {
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel : TransactionStatus
    {
        public int User_Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}