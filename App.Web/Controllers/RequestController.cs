using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class RequestController : Controller
    {
        // GET: Request

       
        public ActionResult Index(string Invce_Num,double Amount)
        {

            return View();
        }
    }
}