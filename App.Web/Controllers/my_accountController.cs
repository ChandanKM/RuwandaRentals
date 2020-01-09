using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;
using Microsoft.AspNet.Identity;
using App.Web.ViewModels;
using System.Net.Http;
using App.UIServices.InterfaceServices;
using App.BusinessObject;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using App.UIServices;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Consumer")]
    public class my_accountController : Controller
    {
        // GET: my_account
        public ActionResult Index()
        {
            string a = Request.Url.ToString();
            string UserId = User.Identity.Name;
            return View();
        }

        public ActionResult my_account()
        {
            string UserId = User.Identity.Name;
            return View();
        }

        public ActionResult Logoff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            //HttpContext.SignOut();
            return RedirectToAction("Signin", "Signin");
        }

        public string GetLoggedInUserId()
        {
            return User.Identity.Name;
        }

        [Authorize]
        public ActionResult Booking_payment_method()
        {
            return View();
        }

        public ActionResult invoice()
        {
            return View();
        }
    }
}