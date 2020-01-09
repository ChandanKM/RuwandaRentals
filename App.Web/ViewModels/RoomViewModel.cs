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
    public class RoomViewModel : TransactionStatus
    {
        #region ROOM

        public int Room_Id { get; set; }
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
        public string Room_Descr { get; set; }
        public string Image_dir { get; set; }
        public int Image_Id { get; set; }


        public int Id { get; set; }
        public int Vndr_Id { get; set; }
        public int Prop_Id { get; set; }
        public int Vndr_Amnt { get; set; }
        public int Lmk_Amnt { get; set; }
        public int camflg_Amnt { get; set; }
        public DateTime Inventory_Date { get; set; }
        public int Inventory_Avail_Rooms { get; set; }
        public int Inventory_Sold_Rooms { get; set; }
        public int Inventory_Hold_Rooms { get; set; }
        public int Inventory_Real_Bal_Rooms { get; set; }

        #endregion


        #region POLICIES

        public int Policy_Id { get; set; }  
        public string Policy_Name { get; set; }
        public string Policy_Descr { get; set; }
        public int Policy_Active_Flag { get; set; }
        
        #endregion


        #region FACILITY

        public int Facility_Id  { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Type { get; set; }
        public string Facility_descr { get; set; }
        public string Facility_Image_dir { get; set; }
        public string Active_flag { get; set; }
        public string Facility_Type1 { get; set; }
        public int IsHeader { get; set; }
        public int FTypecount { get; set; }
        #endregion

    }
}