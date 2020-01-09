using System;
using App.Common;

namespace App.Web.ViewModels
{
    public class UserViewModel : TransactionStatus
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}