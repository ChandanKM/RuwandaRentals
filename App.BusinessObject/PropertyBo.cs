using System;

namespace App.BusinessObject
{
    public class PropertyBo
    {
        public int Prop_Id { get; set; }
        public int Image_Id { get; set; }
        public string Prop_Name { get; set; }
        public string City_Area { get; set; }
        public int City_Id { get; set; }
        //public string City_Name { get; set; }

        public int LocationId { get; set; }
        public String Location_desc { get; set; }

        public int PincodeId { get; set; }
        public string Pricing_Type { get; set; }
        public string Prop_Type { get; set; }
        public string TripAdv { get; set; }

        public string Prop_Overview { get; set; }
        public string Prop_Cin_No { get; set; }
        public string Prop_Addr1 { get; set; }
        public string Prop_Addr2 { get; set; }

        public string Prop_Booking_MailId { get; set; }
        public string Prop_Booking_Mob { get; set; }

        public string Prop_Pricing_MailId { get; set; }
        public string Prop_Pricing_Mob { get; set; }

        public string Prop_Inventory_MailId { get; set; }
        public string Prop_Inventory_Mob { get; set; }

        public string Prop_GPS_Pos { get; set; }
        public string Prop_Star_Rating { get; set; }

        public string Prop_verifiedBy { get; set; }
        public string Prop_Verfied_on { get; set; }
        public string Prop_Approved_By { get; set; }
        public DateTime Prop_Approved_on { get; set; }

        public DateTime Prop_Active_From { get; set; }
        public DateTime Prop_Expires_on { get; set; }
        public string Prop_Active_flag { get; set; }

        public String PropertyCount { get; set; }

        public string Pincode { get; set; }
        public int Vndr_Id { get; set; }

        public int Facility_Id { get; set; }
        public string Facility_Type { get; set; }
        public String Facility_Name { get; set; }

        public int Event_Id { get; set; }

        public string Bank_Name { get; set; }
        public string Bank_Branch_Name { get; set; }

        public string Bank_Branch_Code { get; set; }
        public string Bank_IFC_code { get; set; }
        public string Bank_Accnt_No { get; set; }
        public string Bank_Accnt_Name { get; set; }
        public int Bank_Id { get; set; }
        public string Bank_descr { get; set; }

        public string Event_Name { get; set; }
        public string Event_descr { get; set; }
        public string Event_Start { get; set; }
        public string Event_End { get; set; }
        public string Image_dir { get; set; }
        public string Policy_Name { get; set; }
        public string Policy_descr { get; set; }
        public int VendId { get; set; }
        public int Policy_Id { get; set; }
        public int Room_Id { get; set; }
        public string Room_Checkins { get; set; }
        public string Room_Checkouts { get; set; }
        //new data
        public string Location_Name { get; set; }
        public string City_Name { get; set; }
        public string State_Name { get; set; }
        public string Pin_Code { get; set; }
    }
}
