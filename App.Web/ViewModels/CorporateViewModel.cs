using App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App.Web.ViewModels
{
    public class CorporateViewModel : TransactionStatus
    {
        public string Corp_mailid { get; set; }
        public string Corp_Pswd { get; set; }
        public string Corp_ID { get; set; }
    }
    public class CorporateLoginViewModel : TransactionStatus
    {
        public string Corp_mailid { get; set; }
        public string Corp_Pswd { get; set; }
        public string Corp_ID { get; set; }
    }
    public class CorporateWebLoginViewModel : TransactionStatus
    {
        public string Corp_mailid { get; set; }
        public string Corp_Pswd { get; set; }
        public string returnUrl { get; set; }
    }
}