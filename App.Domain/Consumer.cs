using System;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{
    public class Consumer
    {
        [Key]
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

    public class ConsumerMandet
    {
        [Key]
        public string Cons_First_Name { get; set; }
        public string Cons_Last_Name { get; set; }
        public string Cons_mailid { get; set; }
        public string Cons_Pswd { get; set; }
        public string Cons_Mobile { get; set; }
        public bool emailCheck { get; set; }
        public string Cons_Company { get; set; }
    }

    public class ConsumerLogin
    {
        [Key]
        public string Cons_mailid { get; set; }
        public string Cons_Pswd { get; set; }
    }


    public class ConsumerDetails
    {
        [Key]
        public int Cons_Id { get; set; }
    }
    public class ConsumerForgotpwd
    {
        [Key]
        public string Cons_mailid { get; set; }
    }
    public class PropertyListing
    {
        [Key]
        public string CityMasterId { get; set; }

        public DateTime Room_Checkin { get; set; }
        public DateTime Room_Checkout { get; set; }
        public int No_Of_Rooms { get; set; }
        public string Facilities { get; set; }
        public string Price1 { get; set; }
        public string Price2 { get; set; }
        public string Rating { get; set; }

        public string SortBy { get; set; }
    }
    public class PropertyListingdetails
    {
        [Key]

        public int PropId { get; set; }
        public DateTime Room_Checkin { get; set; }
        public DateTime Room_Checkout { get; set; }
        public int No_Of_Rooms { get; set; }


    }
    public class PropertyListingdetailsRoom
    {
        [Key]
        public int PropId { get; set; }
        public DateTime Room_Checkin { get; set; }
        public DateTime Room_Checkout { get; set; }
        public int No_Of_Rooms { get; set; }



    }
    public class PreBookingDomain
    {
        [Key]

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
        public int redmpt_points { get; set; }
        public int redmpt_value { get; set; }
        public string Promo_Type { get; set; }
        public int Prop_Value { get; set; }
        public string ipaddress { get; set; }
        public string GuestName { get; set; }
        public string Invce_Num { get; set; }
        //public string AllInfo { get; set; }

    }


    public class ConsumerChangePassword
    {
        [Key]
        public int Cons_Id { get; set; }
        public string Curnt_Pswd { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class ConsumerSubscribe
    {
        [Key]
        public int Cons_Id { get; set; }
        public string Email { get; set; }
        public string Ipaddress { get; set; }
    }

    public class ConsumerForm
    {
        public string Cons_First_Name { get; set; }
        public string Cons_Last_Name { get; set; }
        public string Cons_Gender { get; set; }

        public DateTime Cons_Dob { get; set; }
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
        public string Cons_SMS_Gateway { get; set; }
    }

    public class BookNowDetails
    {
        public int Prop_Id { get; set; }
        public int Room_Id { get; set; }
        public DateTime Room_Checkin { get; set; }
        public DateTime Room_Checkout { get; set; }
        public int No_Of_Rooms { get; set; }
    }

    public class FeedBack
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Mobile { get; set; }
        public string Message { get; set; }
        public string Ipaddress { get; set; }
    }
}
