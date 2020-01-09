using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [AllowAnonymous]
      
        public ActionResult Index()
        {
            //if (Session["AuthID"].ToString() == "Vendor")
            //{
            //    System.Web.Security.FormsAuthentication.SignOut();
            //    return RedirectToAction("Index", "Signin");
            //}
            //else
            //{

                return View();
            //}
        }

        [AllowAnonymous]
        public ActionResult Contact_Us()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Feedback()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult terms_conditions()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Cancelation()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Complaints()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Faq()
        {
            return View();
        }
         [AllowAnonymous]
        public ActionResult NewsLatter()
        {
            return View();
        }
         [AllowAnonymous]
         public ActionResult ManageBooking()
         {
             return View();
         }
         [AllowAnonymous]
         public ActionResult PageNotFound()
         {
             return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
         }
         public ActionResult AboutUs()
         {
             return View();
         }
    }
}