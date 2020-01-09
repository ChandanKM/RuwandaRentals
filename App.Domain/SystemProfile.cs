using System;
using System.ComponentModel.DataAnnotations;

namespace App.BusinessObject
{
    public class SystemProfile
    {
        [Key]
        public int Id { get; set; }
        public string Company_Title { get; set; }
        public string Owned_By { get; set; }
        public string CIN_Number { get; set; }
        public string Adr1 { get; set; }
        public string Adr2 { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string Tin_id { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Sms_Url { get; set; }
        public string User_Id { get; set; }
        public int UserProfile_Id { get; set; }
        public string SetupEmail { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
    }
}
