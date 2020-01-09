using System;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{
    public class Param
    {
        [Key]
        public int Id { get; set; }
        public string Vparam_Code { get; set; }
        public string Vparam_Descr { get; set; }
        public string Vparm_Type { get; set; }
        public string Vparam_Val { get; set; }
    }
}
