using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace App.Domain
{
    public class Rooms
    {
        [Key]
        public int Prop_Id { get; set; }
        public int Type_Id { get; set; }
        public string Room_Name { get; set; }
        public string Room_Overview { get; set; }
        public string Room_Adult_occup { get; set; }
        public string Room_Child_occup { get; set; }
        public string Room_Extra_Adul { get; set; }
        public string Room_Standard_rate { get; set; }
        public string Room_Agreed_Availability { get; set; }
        public string Room_Lmk_Rate { get; set; }
        public string Room_camflg { get; set; }
        public string Room_Checkin { get; set; }
        public string Room_Checkout { get; set; }
        public string Room_Grace_time { get; set; }
        public string Room_Max_Thrshold_Disc { get; set; }
        public string Tax_Id { get; set; }
        public string Room_Active_flag { get; set; }
        public string Image_dir { get; set; }
        public int Image_Id { get; set; }  
    }

    public class RoomsEdit
    {
        [Key]
        public int Room_Id { get; set; }
        public int Prop_Id { get; set; }
        public int Type_Id { get; set; }
        public string Room_Name { get; set; }
        public string Room_Overview { get; set; }
        public string Room_Adult_occup { get; set; }
        public string Room_Child_occup { get; set; }
        public string Room_Extra_Adul { get; set; }
        public string Room_Standard_rate { get; set; }
        public string Room_Agreed_Availability { get; set; }
        public string Room_Lmk_Rate { get; set; }
        public string Room_camflg { get; set; }
        public string Room_Checkin { get; set; }
        public string Room_Checkout { get; set; }
        public string Room_Grace_time { get; set; }
        public string Room_Max_Thrshold_Disc { get; set; }
        public string Tax_Id { get; set; }
        public string Room_Active_flag { get; set; }
        public string Image_dir { get; set; }
        public int Image_Id { get; set; }  
    }


    public class Policies
    {
        [Key]
        public int Policy_Id { get; set; }
        public int Prop_Id { get; set; }
        public int Room_Id { get; set; }

        public string Policy_Name { get; set; }
        public string Policy_Descr { get; set; }
        public int Policy_Active_Flag { get; set; }
    }


    public class PoliciesEdit
    {
        [Key]
        public int Id { get; set; }
        public int Policy_Id { get; set; }
        public int Prop_Id { get; set; }
        public int Room_Id { get; set; }

        public string Policy_Name { get; set; }
        public string Policy_Descr { get; set; }
        public int Policy_Active_Flag { get; set; }
    }
    public class request
    {
        [Key]
        public string username { get; set; }

        public string password { get; set; }

        public string hotel_id { get; set; }

        public room room { get; set; }

        //public string id { get; set; }

        public double version { get; set; }       

        //public string rate_id { get; set; }

        public List<dates> dates { get; set; }

        public request()
        {
            //rooms = new List<room>();
            room = new room();
            dates = new List<dates>();
        }
    }

    public class room
    {
        public string id { get; set; }

        public rate rate { get; set; }

        public room()
        {
            rate = new rate();
        }
    }

    public class rate
    {
        public string id { get; set; }

        public rate()
        {

        }
    }

    public class dates
    {
        public string date { get; set; }

        //public int rateid { get; set; }

        public float price { get; set; }

        public int roomstosell { get; set; }

        public int closed { get; set; }

        public int minimumstay { get; set; }

        public int maximumstay { get; set; }

        public int closedonarrival { get; set; }

        public int closedondeparture { get; set; }

        public dates()
        {

        }
    }

    public class Roomrequest
    {
        [Key]
        public string username { get; set; }

        public string password { get; set; }

        public string hotel_id { get; set; }
    }

    public class roomresponse
    {
        [Key]
       public List<roomtype> roomtypes { get; set; }

        public roomresponse()
        {
            roomtypes = new List<roomtype>();
        }
    }

    public class roomtype
    {
        [Key]
        public int roomtypeid { get; set; }

        public string roomtypename { get; set; }

        public string rateplanid { get; set; }

        public string  rateplanname { get; set; }

      
    }

    public class RoomFacility
    {
        [Key]
        public int Room_Id { get; set; }
        public int Facility_Id { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Type { get; set; }
        public string Facility_descr { get; set; }
        public string Facility_Image_dir { get; set; }
        public string Facility_Active_flag { get; set; }
        public string imageurl { get; set; }
        public int Prop_Id { get; set; }
    }

    public class RoomFacilityEdit
    {
        [Key]
        public int Id { get; set; }
        public int Room_Id { get; set; }
        public int Facility_Id { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Type { get; set; }
        public string Facility_descr { get; set; }
        public string Facility_Image_dir { get; set; }
        public string Facility_Active_flag { get; set; }
        public int Prop_Id { get; set; }
    }

    #region RoomRateCalender
    public class RateCalender
    {
        [Key]
        int Inv_Id { get; set; }
        int Price { get; set; }
        int Available { get; set; }
    }
    #endregion
}
