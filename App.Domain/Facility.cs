using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{

    public class Facility
    {
        [Key]
        public int Facility_Id { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Type { get; set; }
        public string Facility_descr { get; set; }
        public string Facility_Image_dir { get; set; }
        public string Facility_Active_flag { get; set; }
    }

    public class FacilityEdit
    {
        [Key]
        public int Facility_Id { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Type { get; set; }
        public string Facility_descr { get; set; }
        public string Facility_Image_dir { get; set; }
        public string Facility_Active_flag { get; set; }
    }

}
