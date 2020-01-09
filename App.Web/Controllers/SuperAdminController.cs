using System;
using System.Web.Mvc;
using App.Web.ViewModels;
using App.Web;
using App.UIServices;
using System.Web;
using System.IO;
using App.UIServices.InterfaceServices;
using System.Collections.Generic;
using App.DataAccess;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using Omu.ValueInjecter;
using System.Net.Http;
using App.BusinessObject;

namespace App.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin
        readonly ILoginServices _loginService;
        public SuperAdminController(ILoginServices loginService)
        {

            _loginService = loginService;
        }
        private LoginBo BuiltLoginBo(LoginViewModel loginVm)
        {
            return (LoginBo)new LoginBo().InjectFrom(loginVm);
        }
        public ActionResult Index()
        {
            return View();
        }
        public string GetLoginUserId()
        {

            string Uid = User.Identity.Name;

            return Uid;
        }
        public string GetNoOfPropertiesCreated(int UID)
        {
            string Uid = User.Identity.Name;
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@UserID",UID),//0
                 
			};
            string PropCount = "";

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "[proc_NO_OF_Properties_Created]", Params);
            List<object> lstcityloc = new List<object>();
            while (reader.Read())
            {
                PropCount = reader["avail"].ToString();
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return PropCount;
        }
        public string GetLoginAuthId()
        {
            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@UserId",Uid),//0
			};
            string VendorId = "";

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectVendorId", Params);
            List<object> lstcityloc = new List<object>();
            while (reader.Read())
            {
                VendorId = reader["Authority_Id"].ToString();
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return VendorId;
        }
        public string GetLoginName()
        {
            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@UserId",Uid),//0
			};
            string VendorId = "";

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectSuperAdminName]", Params);
            List<object> lstcityloc = new List<object>();
            while (reader.Read())
            {
                VendorId = reader["Company_Title"].ToString();
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return VendorId;
        }
        public ActionResult Occupancy()
        {
            return View();
        }
        public ActionResult Consumer_Report()
        {
            return View();
        }

        public ActionResult CCAvenue_Charges()
        {
            return View();
        }
        public ActionResult LMK_Margin_Reports()
        {
            return View();
        }
        public ActionResult AdminProfile()
        {
            string Uid = User.Identity.Name;
            return RedirectToAction("SystemProfileEdit/" + Uid + "", "SuperAdmin");
            //   return View();
        }

        public ActionResult Profiles()
        {
            return View();
        }
        public ActionResult VendorProfiles()
        {
            return View();
        }
        public ActionResult PropertyUserProfiles()
        {
            return View();
        }
        public ActionResult ProfileProperties()
        {
            return View();
        }
        public ActionResult City_Location()
        {
            return View();
        }

        public ActionResult Facilities()
        {
            return View();
        }

        public ActionResult Parameters()
        {
            return View();
        }
        public ActionResult Room_Types()
        {
            return View();
        }
        public ActionResult CreateFacility()
        {
            return View();
        }
        public ActionResult EditFacility(int Id)
        {
            return View();
        }
        public ActionResult SystemProfile(int Id)
        {
            return View();
        }
        public ActionResult SystemProfileEdit(int Id)
        {
            return View();
        }
        public ActionResult Room_Type()
        {
            return View();
        }
        public ActionResult Tax_Report()
        {
            return View();
        }
        public ActionResult Properties()
        {
            return View();
        }
        public ActionResult RoomRateCal()
        {
            return View();
        }
        public ActionResult Upcomming_Bookings()
        {
            return View();
        }
        public ActionResult DashBoard()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult SuperAdminForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("SuperAdmin"))
                    return RedirectToAction("DashBoard", "SuperAdmin");
                else
                    return RedirectToAction("MultipleLoginError", "Error");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewmodel, string returnUrl)
        {

            //if (!User.Identity.IsAuthenticated)
            //{

            if (!ModelState.IsValid)
            {
                return View(loginViewmodel);
            }
            try
            {
                var login = BuiltLoginBo(loginViewmodel);
                List<string> lst = _loginService.AuthenticateUser(login);
                string user_Id = string.Empty, Authority_Id = string.Empty, user_Code = string.Empty, user_type = string.Empty;
                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i = i + 3)
                    {
                        user_Id = lst[i];
                        Authority_Id = lst[i + 1];
                        user_type = lst[i + 2];
                    }


                    switch (Authority_Id)
                    {

                        case "1":
                          
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            if (user_type == "0")
                                return RedirectToAction("SystemProfile/" + user_Id + "", "SuperAdmin");
                            else
                                return RedirectToAction("DashBoard", "SuperAdmin");
                        // return RedirectToAction("SystemProfileEdit/" + user_Id + "", "SuperAdmin");
                        case "2":
                           
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            //if (user_type == "0")
                            //    return RedirectToAction("SystemProfile/" + user_Id + "", "SuperAdmin");
                            //else
                                // return RedirectToAction("SystemProfileEdit/" + user_Id + "", "SuperAdmin");
                                return RedirectToAction("DashBoard", "SuperAdmin");

                        default:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(loginViewmodel);
                    }
                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    //return View(loginViewmodel);
                    return RedirectToAction("DashBoard", "SuperAdmin");
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return RedirectToAction("Index");
            }




        }

        [AllowAnonymous]
        public ActionResult Logoff()
        {
            //Session["LoginUser"] = "SuperAdmin";
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Login", "SuperAdmin");
        }

        public ActionResult CC_Avenue()
        {
            return View();
        }

        public void Export()
        {

            string data = Request.Form["data"];
            data = HttpUtility.UrlDecode(data);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Booking.xls");
            Response.Charset = "";
            Response.ContentType = "application/excel";
            System.Web.HttpContext.Current.Response.Write(data);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();

            //return View();
        }

        public void marginExport()
        {

            string data = Request.Form["data"];
            data = HttpUtility.UrlDecode(data);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Revenue.xls");
            Response.Charset = "";
            Response.ContentType = "application/excel";
            System.Web.HttpContext.Current.Response.Write(data);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();

            //return View();
        }


        public ActionResult Corporate()
        {
            return View();
        }

        public ActionResult CorporateReport()
        {
            return View();
        }

        #region Report
        public ActionResult ReportTransactions()
        {
            return View();
        }
        #endregion
    }
}