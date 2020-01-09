using App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.ViewModels
{
    public class PincodeViewModel : TransactionStatus
    {
        public int PincodeId { get; set; }
        public string Pincode { get; set; }
    }
}