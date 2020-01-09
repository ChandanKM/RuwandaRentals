using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using App.Common;

namespace App.Web.ViewModels
{
    public class RoomTypeViewModel : TransactionStatus
    {
        public int Room_TypeId { get; set; }
        public string Room_Name { get; set; }
        public string Room_Descr { get; set; }
        public string Room_Active_flag { get; set; }

    }
    public class RoomTypeViewModelSusspend : TransactionStatus
    {
        public int Room_TypeId { get; set; }
      
    }
}