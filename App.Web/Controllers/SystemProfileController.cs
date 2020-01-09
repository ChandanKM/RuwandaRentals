using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Web.ViewModels;
namespace App.Web.Controllers
{
    public class SystemProfileController : Controller
    {
        // GET: SystemProfile
        public ActionResult Add()
        {
            var cookieId = new HttpCookie("userprofileId");
            cookieId.Value=User.Identity.Name;
            Response.Cookies.Add(cookieId);
            return View();
        }

        public ActionResult Edit(int Id)
        {
            return View();
        }
    }
}