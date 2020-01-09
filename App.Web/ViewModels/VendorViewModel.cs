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
    public class VendorViewModel : TransactionStatus
    {

        public String Vndr_Name { get; set; }
        public String Image_dir { get; set; }
        public String Vndr_Cinno { get; set; }
        public String Vndr_Addr1 { get; set; }
        public int Vndr_Id { get; set; }
        public int PropId { get; set; }
        public string City_Area { get; set; }
        public int City_Id { get; set; }

        public string Vndr_Gps_Pos { get; set; }
        public string Vndr_Overview { get; set; }
        public string Vndr_Contact_person { get; set; }
        public string Vndr_Contact_Email { get; set; }
        public string Vndr_Contact_Nos { get; set; }
        public string Vndr_Contact_Mobile { get; set; }
        public string Vndr_Contact_Designation { get; set; }
        public string Vndr_Alternate_Mobile { get; set; }
        public string Vndr_Alternate_Designation { get; set; }
        public string Vndr_Alternate_person { get; set; }
        public string Vndr_Alternate_Email { get; set; }
        public string Vndr_Alternate_Nos { get; set; }
        public string Vndr_Lanline_Nos { get; set; }
        public string Vndr_Mobile_Nos { get; set; }
        public DateTime Vndr_created_on { get; set; }
        public DateTime Vndr_Activated_on { get; set; }
        public string Vndr_Commercial_Estab_Flag { get; set; }
        public string Vndr_Verfied_By { get; set; }
        public DateTime Vndr_Verfied_on { get; set; }
        public string Vndr_Approved_By { get; set; }
        public DateTime Vndr_Approved_on { get; set; }
        public string Vndr_Active_Flag { get; set; }
        public int UserProfile_Id { get; set; }
    }


    public class VendorIDViewModel : TransactionStatus
    {
        public int Vendor_ID { get; set; } 
    }
}