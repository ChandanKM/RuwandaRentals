using System;
using System.Web.Mvc;
using App.Web.ViewModels;
using App.Web;
using App.UIServices;

namespace App.Web.Controllers
{
    public class PincodeController : Controller
    {
        public ActionResult Create()
        {
            var pincodeViewModel = new PincodeViewModel();
            return View(pincodeViewModel);
        }

        public ActionResult Bind()
        {
            return View();
        }

        public ActionResult Edit(int Id)
        {
            return View();
        }
    }
}