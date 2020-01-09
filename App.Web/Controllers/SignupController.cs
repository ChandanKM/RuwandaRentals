using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class SignupController : Controller
    {
        // GET: Signup
        public ActionResult Index(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
                SigninController.returnUrlProperty = returnUrl;
            }
            else
                SigninController.returnUrlProperty = null;
            return View();
        }

        [AllowAnonymous]
        public ActionResult SignUp(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
               SigninController.returnUrlProperty = returnUrl;
            }
            else
                SigninController.returnUrlProperty = null;
            return View();
        }
    }
}