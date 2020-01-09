using System;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{
    public class Vendor
    {
        [Key]
        public int Vndr_Id { get; set; }
        public String Vndr_Name { get; set; }
        public String Vndr_Cinno { get; set; }
        public String Vndr_Addr1 { get; set; }
        public String Vendor_Address2 { get; set; }

        #region dropdownlist
        //public List<CityDTO> CityList { get; set; }
        //public List<LocationDTO> LocationList { get; set; }
        //public List<PincodeDTO> PincodeList { get; set; }
        public string Cityvalue { get; set; }
        public string locationvalue { get; set; }
        public string pincodevalue { get; set; }
        public int City_Id { get; set; }
        public int Location_Id { get; set; }
        public int Pincode_Id { get; set; }
        #endregion


        public string City_Area { get; set; }
        public string Vndr_Lanline_Nos { get; set; }
        public string Vndr_Mobile_Nos { get; set; }
        public int Vndr_Prop_Count { get; set; }
        public string Vndr_Gps_Pos { get; set; }
        public string Vndr_Overview { get; set; }
        public string Vndr_Contact_person { get; set; }


        public string Vndr_Contact_Email { get; set; }
        public string Vndr_Contact_Nos { get; set; }

        public string Vndr_Alternate_person { get; set; }

        public string Vndr_Alternate_Email { get; set; }
        public string Vndr_Alternate_Nos { get; set; }

      
        public DateTime Vndr_created_on { get; set; }
        public DateTime Vndr_Activated_on { get; set; }
        public string Vndr_Commercial_Estab_Flag { get; set; }
        public string Vndr_Verfied_By { get; set; }
        public DateTime Vndr_Verfied_on { get; set; }
        public string Vndr_Approved_By { get; set; }
        public DateTime Vndr_Approved_on { get; set; }
        public string Vndr_Active_Flag { get; set; }

        public int UserProfile_Id { get; set; }
        public string Vndr_Contact_Mobile { get; set; }
        public string Vndr_Contact_Designation { get; set; }
        public string Vndr_Alternate_Mobile { get; set; }
        public string Vndr_Alternate_Designation { get; set; }
    }

    public class VendorEdit
    {

    
   
        [Key]
        public int Vndr_Id { get; set; }
        public String Vndr_Name { get; set; }
        public String Vndr_Cinno { get; set; }
        public String Vndr_Addr1 { get; set; }
        public String Vendor_Address2 { get; set; }
        public string City_Area { get; set; }
        #region dropdownlist
        //public List<CityDTO> CityList { get; set; }
        //public List<LocationDTO> LocationList { get; set; }
        //public List<PincodeDTO> PincodeList { get; set; }
        public string Cityvalue { get; set; }
        public string locationvalue { get; set; }
        public string pincodevalue { get; set; }
        public int City_Id { get; set; }
        public int Location_Id { get; set; }
        public int Pincode_Id { get; set; }
        #endregion
 
        public string Vndr_Lanline_Nos { get; set; }
        public string Vndr_Mobile_Nos { get; set; }
        public int Vndr_Prop_Count { get; set; }
        public string Vndr_Gps_Pos { get; set; }
        public string Vndr_Overview { get; set; }
        public string Vndr_Contact_person { get; set; }


        public string Vndr_Contact_Email { get; set; }
        public string Vndr_Contact_Nos { get; set; }

        public string Vndr_Alternate_person { get; set; }

        public string Vndr_Alternate_Email { get; set; }
        public string Vndr_Alternate_Nos { get; set; }
     
        public DateTime Vndr_created_on { get; set; }
        public DateTime Vndr_Activated_on { get; set; }
        public string Vndr_Commercial_Estab_Flag { get; set; }
        public string Vndr_Verfied_By { get; set; }
        public DateTime Vndr_Verfied_on { get; set; }
        public string Vndr_Approved_By { get; set; }
        public DateTime Vndr_Approved_on { get; set; }
        public string Vndr_Active_Flag { get; set; }
        public string Vndr_Alternate_Mobile { get; set; }
        public string Vndr_Alternate_Designation { get; set; }
        public string Vndr_Contact_Mobile { get; set; }
        public string Vndr_Contact_Designation { get; set; }
    }
}
