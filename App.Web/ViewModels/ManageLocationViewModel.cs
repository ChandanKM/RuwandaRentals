using System;
using App.Common;

namespace App.Web.ViewModels
{
    public class ManageLocationViewModel : TransactionStatus
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
    }
}