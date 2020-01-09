using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class Search_ResultsController : Controller
    {
        // GET: search_results
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult More_Results()
        {
            return View();
        }
    }
}