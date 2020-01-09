using System;
using System.Web.Mvc;
using App.Web.ViewModels;
using App.Web;
using App.UIServices;

namespace App.Web.Controllers
{
    public class CityController:Controller
    {

        public ActionResult Create()
        {
            var cityViewModel = new CityViewModel();

            return View(cityViewModel);
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