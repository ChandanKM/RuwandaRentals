using System;
using System.ComponentModel.DataAnnotations;
namespace App.Domain
{
    public class RoomType
    {
        [Key]
        public int Room_TypeId { get; set; }
        public string Room_Name { get; set; }
        public string Room_Descr { get; set; }
        public string Room_Active_flag { get; set; }
    }
}
