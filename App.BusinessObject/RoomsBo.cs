using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BusinessObject
{
    public class RoomsBo
    {
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

    public class RoomsEditBo
    {
       
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


    public class PoliciesBo
    {       
        public int Policy_Id { get; set; }
        public int Prop_Id { get; set; }
        public int Room_Id { get; set; }
 
        public string Policy_Name { get; set; }
       public string Policy_Descr { get; set; }
       public int Policy_Active_Flag { get; set; }
    }


    public class PoliciesEditBo
    {
        public int Id { get; set; }
        public int Policy_Id { get; set; }
        public int Prop_Id { get; set; }
        public int Room_Id { get; set; }

        public string Policy_Name { get; set; }
        public string Policy_Descr { get; set; }
        public int Policy_Active_Flag { get; set; }
    }


    public class RoomFacilityBo
    {
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

    public class RoomFacilityEditBo
    {
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
    public class RateCalenderBo
    {
        int Inv_Id { get; set; }
        int Price { get; set; }
        int Available { get; set; }
    }
    #endregion
}
