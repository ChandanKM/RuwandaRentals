using System;
using System.Collections.Generic;

namespace App.BusinessObject
{
    public class ApplicationErrorLogBo
    {
        public int ID { get; set; }
        public string Error { get; set; }
        public string Stacktrace { get; set; }
        public string InnerException { get; set; }
        public string Source { get; set; }
        public DateTime ExceptionDateTime { get; set; }
    }


}


