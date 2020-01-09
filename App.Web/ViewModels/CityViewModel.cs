using System;
using App.Common;

namespace App.Web.ViewModels
{
    public class CityViewModel : TransactionStatus
    {
        public int City_Id { get; set; }
        public string City_Name { get; set; }
    }
}