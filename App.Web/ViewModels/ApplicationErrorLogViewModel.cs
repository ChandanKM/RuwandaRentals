using System;
using App.Common;

namespace App.Web.ViewModels
{
    public class ApplicationErrorLogViewModel : TransactionStatus
    {
        public string Error { get; set; }
        public string Stacktrace { get; set; }
        public string InnerException { get; set; }
        public string Source { get; set; }      
    }
}

