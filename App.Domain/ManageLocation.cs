using System;
using System.ComponentModel.DataAnnotations;
namespace App.BusinessObject
{
    public class ManageLocation
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
    }
}
