using System;
using App.Common;

namespace App.Web.ViewModels
{
    public class FacilityViewModel:TransactionStatus
    {
        public int Facility_Id { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Type { get; set; }
        public string Facility_descr { get; set; }
        public string Facility_Image_dir { get; set; }
        public string Facility_Active_flag { get; set; }
    }
}