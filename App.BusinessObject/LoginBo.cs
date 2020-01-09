using System;

namespace App.BusinessObject
{
    public class LoginBo
    {
        public string UserId { get; set; }
        public string Pswd { get; set; }
    }

    public class ForgotPasswordBo
    {
        public string Email { get; set; }
        public int Id { get; set; }
    }
    public class EmailMasterBo
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
        public string Pop { get; set; }
        public int Port { get; set; }
        public bool DefaultSsl { get; set; }
    }


    public class ResetPasswordBo
    {
        public int User_Id { get; set; }
        public string Password { get; set; }
    }
}
