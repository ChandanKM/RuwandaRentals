using System;
using App.Common;


namespace App.Web.ViewModels
{
    public class ParamViewModel : TransactionStatus
    {
        public int Id { get; set; }
        public int Vndr_Id { get; set; }
        public string Vparam_Code { get; set; }
        public string Vparam_Descr { get; set; }
        public string Vparm_Type { get; set; }
        public string Vparam_Val { get; set; }
    }
}