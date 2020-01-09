using System;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{
    public class City
    {
        [Key]
        public int City_Id { get; set; }
        public string City_Name { get; set; }
    }

    public class City1
    {
        [Key]
        public int City_Id { get; set; }
        public string City_Name { get; set; }
    }
}
