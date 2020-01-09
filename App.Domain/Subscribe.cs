using System;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{
    public class Subscribe
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Active_flag { get; set; }
        public DateTime Date { get; set; }
        public string Ipaddress { get; set; }
    }
}
