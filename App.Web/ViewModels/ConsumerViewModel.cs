using System;
using App.Common;
using System.ComponentModel.DataAnnotations;

namespace App.Web.ViewModels
{
    public class ConsumerViewModel : TransactionStatus
    {
        public string Cons_First_Name { get; set; }
        public string Cons_Last_Name { get; set; }
        public string Cons_Gender { get; set; }

        public string Cons_Dob { get; set; }
        public string Cons_mailid { get; set; }
        public string Cons_Pswd { get; set; }

        public string Cons_Mobile { get; set; }
        public string Cons_Addr1 { get; set; }
        public string Cons_Addr2 { get; set; }
        public string Cons_City { get; set; }
        public string Cons_Area { get; set; }
        public int Cons_Pincode { get; set; }

        public string Cons_Company { get; set; }
        public string Cons_Company_Id { get; set; }
        public string Cons_Reference { get; set; }

        public int Cons_Affiliates_Id { get; set; }
        public int Cons_Loyalty_Id { get; set; }
        public int Cons_Earned_Loyalpoints { get; set; }

        public int Cons_Redeemed_Loyalpoints1 { get; set; }
        public string Cons_Ipaddress { get; set; }
        public string Cons_regist_On { get; set; }
        public string Cons_Active_flag { get; set; }
        public int Cons_Id { get; set; }
    }

    public class ConsumerMandetViewModel : TransactionStatus
    {
        public string Cons_First_Name { get; set; }
        public string Cons_Last_Name { get; set; }
        public string Cons_mailid { get; set; }
        public string Cons_Pswd { get; set; }
        public string ConfirmPassword { get; set; }
        public string Cons_Mobile { get; set; }
        public string Cons_Company{ get; set; }

        public bool emailCheck { get; set; }


    }

    public class ConsumerLoginViewModel : TransactionStatus
    {
        [Required(ErrorMessage = "Please Enter Email Id")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string Cons_mailid { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Cons_Pswd { get; set; }
        public string Cons_ID { get; set; }

    }

    public class ConsumerChangePasswordViewModel : TransactionStatus
    {
        public int Cons_Id { get; set; }
        public string Curnt_Pswd { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ConsumerSubscribeViewModel : TransactionStatus
    {
        public int Cons_Id { get; set; }
        public string Email { get; set; }
        public string Ipaddress { get; set; }
    }
    public class ConsumerFormViewModel : TransactionStatus
    {
        public string Cons_First_Name { get; set; }
        public string Cons_Last_Name { get; set; }
        public string Cons_Gender { get; set; }

        public DateTime Cons_Dob { get; set; }
        public string Cons_mailid { get; set; }
        public string Cons_Pswd { get; set; }
        public string Cons_Subject { get; set; }
        public string Cons_Body { get; set; }
      

        public string Cons_Mobile { get; set; }
        public string Cons_Addr1 { get; set; }
        public string Cons_Addr2 { get; set; }
        public string Cons_City { get; set; }
        public string Cons_Area { get; set; }
        public int Cons_Pincode { get; set; }

        public string Cons_Company { get; set; }
        public string Cons_Company_Id { get; set; }
        public string Cons_Reference { get; set; }

        public int Cons_Affiliates_Id { get; set; }
        public int Cons_Loyalty_Id { get; set; }
        public int Cons_Earned_Loyalpoints { get; set; }

        public int Cons_Redeemed_Loyalpoints1 { get; set; }
        public string Cons_Ipaddress { get; set; }
        public string Cons_regist_On { get; set; }
        public string Cons_Active_flag { get; set; }
        public int Cons_Id { get; set; }
        public string Cons_SMS_Gateway { get; set; }
    }
    //public class PropertyViewModel : TransactionStatus
    //{
    //    public int CityId { get; set; }
    //    public int LocationId { get; set; }
    //    public DateTime Room_Checkin { get; set; }
    //    public DateTime Room_Checkout { get; set; }
    //    public int No_Of_Rooms { get; set; }



    //}
    public class PropertyDetailsViewModel : TransactionStatus
    {
        public int PropId { get; set; }
        public DateTime Room_Checkin { get; set; }
        public DateTime Room_Checkout { get; set; }
        public int No_Of_Rooms { get; set; }



    }
    public class PropertyDetailsViewModel1 : TransactionStatus
    {
        public int PropId { get; set; }
        public DateTime Room_Checkin { get; set; }
        public DateTime Room_Checkout { get; set; }
        public int No_Of_Rooms { get; set; }



    }
    public class ConsumerDetailsViewModel : TransactionStatus
    {
        public int Cons_Id { get; set; }




    }
    public class ConsumerForgotpwdViewModel : TransactionStatus
    {
        public string Cons_mailid { get; set; }


    }
    public class PrebookingViewModel : TransactionStatus
    {

        public int Vndr_ID { get; set; }
        public int PropId { get; set; }
        public int RoomID { get; set; }
        public int Cons_Id { get; set; }

        public DateTime Room_Checkin { get; set; }
        public DateTime Room_Checkout { get; set; }
        public int Room_Count { get; set; }
        public int prop_room_rate { get; set; }
        public int camo_room_rate { get; set; }
        public float tax_amnt { get; set; }
        public float net_amt { get; set; }
        public string Invce_note { get; set; }
        public string Trans_No { get; set; }
        public string paid_status { get; set; }
        public string credit_debit_card { get; set; }
        public int card_no { get; set; }
        public string card_type { get; set; }
        public string redmpt_points { get; set; }
        public string redmpt_value { get; set; }
        public string Promo_Type { get; set; }
        public string Prop_Value { get; set; }
        public string ipaddress { get; set; }
        public string GuestName { get; set; }
        public string Invce_Num { get; set; }



    }


    public class BookNowDetailsViewModel : TransactionStatus
    {
        public int Prop_Id { get; set; }
        public int Room_Id { get; set; }
        public DateTime Room_Checkin { get; set; }
        public DateTime Room_Checkout { get; set; }
        public int No_Of_Rooms { get; set; }
    }

    public class FeedBackViewModel : TransactionStatus
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Mobile { get; set; }
        public string Message { get; set; }
        public string Ipaddress { get; set; }
    }

    public class ConsumerWebLoginViewModel : TransactionStatus
    {
        public string Cons_mailid { get; set; }
        public string Cons_Pswd { get; set; }
        public string returnUrl { get; set; }
    }
}