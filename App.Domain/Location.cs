using System;
using System.ComponentModel.DataAnnotations;


namespace App.Domain
{
    public class Lmk_Location
    {
        [Key]
        public string Location_desc { get; set; }
    }

    public class EditLocation
    {
        [Key]
        public int Location_Id { get; set; }
        public string Location_desc { get; set; }
    }
}
