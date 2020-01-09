using App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.ViewModels
{
    public class CCAvenueViewModel : TransactionStatus
    {
        public int Cav_Id { get; set; }
        public string Cav_Name { get; set; }
        public float Cav_Percent { get; set; }
        public string Cav_Descr { get; set; }
        public string Cav_Ipaddress { get; set; }
        public string Cav_Modified_On { get; set; }
        public string Cav_Regist_On { get; set; }
    }
}