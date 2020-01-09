using System;
using System.Web.Mvc;
using App.Web.ViewModels;
using App.Web;
using App.UIServices;

namespace App.Web.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        // GET: Location
        public ActionResult Create()
        {
            var LocationViewModel = new LocationViewModel();
            return View(LocationViewModel);
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