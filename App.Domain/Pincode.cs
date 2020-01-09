using System;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{
    public class Lmk_Pincode
    {
        [Key]
        public string Pincode { get; set; }
    }

    public class EditPincode
    {
        [Key]
        public int PincodeId { get; set; }
        public string Pincode { get; set; }
    }
}
