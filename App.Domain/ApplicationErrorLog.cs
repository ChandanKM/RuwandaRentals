using System;
using System.ComponentModel.DataAnnotations;
namespace App.Domain
{
    public class ApplicationErrorLog
    {
        [Key]
        public int ID { get; set; }
        public string Error { get; set; }
        public string Stacktrace { get; set; }
        public string InnerException { get; set; }
        public string Source { get; set; }
        public DateTime ExceptionDateTime { get; set; }
    }
}

