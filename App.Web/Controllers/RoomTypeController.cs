using System;
using System.Web.Mvc;
using App.Web.ViewModels;
using App.Web;
using App.UIServices;

namespace App.Web.Controllers
{
    [Authorize]
    public class RoomTypeController : Controller
    {
        // GET: RoomType

        public ActionResult Index()
        {
            return View();
        }
    }
}