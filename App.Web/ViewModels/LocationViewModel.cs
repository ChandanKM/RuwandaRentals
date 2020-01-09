using App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.ViewModels
{
    public class LocationViewModel:TransactionStatus
    {
       // public string Id { get; set; }
        public string Location_Id { get; set; }
        public string Location_desc { get; set; }
    }
}