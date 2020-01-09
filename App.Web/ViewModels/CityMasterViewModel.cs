using System;
using App.Common;

namespace App.Web.ViewModels
{
    public class CityMasterViewModel : TransactionStatus
    {
        public int City_Id { get; set; }
        public int location_Id { get; set; }
        public int Pincode_Id { get; set; }     
    }
}