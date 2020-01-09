using System;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{
    public class CityMaster
    {
        [Key]
        public int City_Id { get; set; }
        public int location_Id { get; set; }
        public int Pincode_Id { get; set; }     
    }
   }
