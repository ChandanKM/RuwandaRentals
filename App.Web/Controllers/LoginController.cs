using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;
using App.Web.ViewModels;
using System.Net.Http;
using App.UIServices.InterfaceServices;
using App.BusinessObject;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using App.UIServices;
namespace App.Web.Controllers
{
    [Authorize]
    [ValidateInput(false)]
    public class LoginController : Controller
    {
        // GET: Login
        readonly ILoginServices _loginService;

        public LoginController(ILoginServices loginService)
        {
            _loginService = loginService;
        }


        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel loginViewmodel, string returnUrl)
        {

            //if (User.Identity.IsAuthenticated)
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
                    Session["VendorSession"] = "Vendor";
                    switch (Authority_Id)
                    {
                        case "2":
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            if (user_type == "0")
                                return RedirectToAction("Index/" + Authority_Id + "", "UserProfile");
                            else
                                return RedirectToAction("Index/" + Authority_Id + "", "UserProfile");

                        case "1":
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            if (user_type == "0")
                                return RedirectToAction("Index/" + Authority_Id + "", "UserProfile");
                            else
                                return RedirectToAction("Index/" + Authority_Id + "", "UserProfile");

                        case "3":
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            if (user_type == "0")
                                return RedirectToAction("Create", "Vendor", null);
                            else
                                return RedirectToAction("BindDetails", "Vendor", new { id = user_type });

                        case "4":
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            if (user_type == "0")
                                return RedirectToAction("Index", "UserProfile", null);
                            else
                                return RedirectToAction("Index", "UserProfile", null);
                        case "5":
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            if (user_type == "0")
                                return RedirectToAction("Index", "UserProfile", null);
                            else
                                return RedirectToAction("Index", "UserProfile", null);

                        default:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(loginViewmodel);
                    }
                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(loginViewmodel);
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return RedirectToAction("Index");
            }

            //}
            //else
            //{
            //    return RedirectToAction("Index");
            //}

        }

        public ActionResult Logoff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }


        private LoginBo BuiltLoginBo(LoginViewModel loginVm)
        {
            return (LoginBo)new LoginBo().InjectFrom(loginVm);
        }

        [AllowAnonymous]
        public ActionResult CreatePassword(int Id)
        {
            return View();
        }
    }
}