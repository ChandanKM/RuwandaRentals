using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BusinessObject
{

    public class Customer
    {
        public string address { get; set; }
        public string city { get; set; }
        public string company { get; set; }
        public string countrycode { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string remarks { get; set; }
        public string telephone { get; set; }
        public string zip { get; set; }
    }

    public class Price
    {
        public string date { get; set; }
        public string rate_id { get; set; }
        public string amount { get; set; }
    }

    public class Room
    {
        public string arrival_date { get; set; }
        public string currencycode { get; set; }
        public string departure_date { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public List<Price> price { get; set; }
        public string totalprice { get; set; }
        public string remarks { get; set; }
        public string guest_name { get; set; }
        public string numberofguests { get; set; }
    }

    public class Reservation
    {
        public string commissionamount { get; set; }
        public string deposit { get; set; }
        public string currencycode { get; set; }
        public Customer customer { get; set; }
        public string date { get; set; }
        public string hotel_id { get; set; }
        public string hotel_name { get; set; }
        public string id { get; set; }
        public List<Room> room { get; set; }
        public string status { get; set; }
        public string time { get; set; }
        public string totalprice { get; set; }
    }

    public class Reservations
    {
        public List<Reservation> reservation { get; set; }
    }

    public class RootObject
    {
        public Reservations reservations { get; set; }
    }

    public class reservationsresponse
    {
        public string error { get; set; }
        public string updated { get; set; }
    
    }


}
