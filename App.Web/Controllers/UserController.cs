using System;
using System.Web.Mvc;
using App.Web.ViewModels;
using App.Web;
using App.UIServices;
namespace App.Web.Controllers
{

    public class UserController : Controller
    {

        public ActionResult Create()
        {
            var userViewModel = new UserViewModel();

            return View(userViewModel);
        }

        public ActionResult Bind()
        {
          
            return View();
        }

        public ActionResult  Edit(int Id)
        {

            return View();
        }
   
        public ActionResult Delete(int Id)
        {

            return View();
        }

    }
}