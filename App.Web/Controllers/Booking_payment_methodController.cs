using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
   [Authorize(Roles = "Consumer")]
    public class Booking_payment_methodController : Controller
    {
        // GET: Booking_payment_method
        public ActionResult Index()
        {
            return View();
        }
    }
}