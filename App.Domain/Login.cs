using System;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{
    public class Login
    {
        public string UserId { get; set; }
        public string Pswd { get; set; }
    }

    public class ForgotPassword
    {
        public string Email { get; set; }
        public int Id { get; set; }
    }
   
    public class EmailMaster
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
        public string Pop { get; set; }
        public int Port { get; set; }
        public bool DefaultSsl { get; set; }
    }

    public class ResetPassword
    {
        public int User_Id { get; set; }
        public string Password { get; set; }
    }
}
