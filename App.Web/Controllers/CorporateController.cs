using App.BusinessObject;
using App.UIServices;
using App.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Corporate")]
    [ValidateInput(false)]
    public class CorporateController : Controller
    {
        public static string returnUrlProperty { get; set; }

        readonly ICorporateService _corporateService;

        public CorporateController(ICorporateService corporateService)
        {
            _corporateService = corporateService;
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }


        [Authorize]
        public ActionResult Booking_payment_method()
        {
            return View();
        }
        public ActionResult payment()
        {
            return View();
        }
        public ActionResult my_account()
        {
            string UserId = User.Identity.Name;
            return View();
        }

        public ActionResult invoice()
        {
            return View();
        }

        //Consumer Account Login System

        [AllowAnonymous]
        public ActionResult Signin(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Corporate"))
                    return RedirectToAction("my_account", "Corporate");
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
        public ActionResult Signin(CorporateLoginViewModel corporateloginViewmodel, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return View(corporateloginViewmodel);
            }
            try
            {
                var loginBo = BuiltCorporateLoginBo(corporateloginViewmodel);
                DataSet data = _corporateService.CorporateLogin(loginBo);
                string user_Id = string.Empty;
                if (data.Tables.Count > 0)
                {

                    user_Id = data.Tables[0].Rows[0]["Corp_mailid"].ToString();

                    if (user_Id != "0")
                    {
                        System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            if (!string.IsNullOrEmpty(returnUrlProperty))
                            {
                                returnUrl = returnUrlProperty;
                            }
                        }
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                        }

                        return RedirectToAction("my_account", "Corporate");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(corporateloginViewmodel);
                    }

                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(corporateloginViewmodel);
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return RedirectToAction("Signin");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult Logoff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            //HttpContext.SignOut();
            return RedirectToAction("Signin");
        }

        public string GetLoggedInUserId()
        {
            return User.Identity.Name;
        }

        private CorporateLoginBo BuiltCorporateLoginBo(CorporateLoginViewModel loginVm)
        {
            return (CorporateLoginBo)new CorporateLoginBo().InjectFrom(loginVm);
        }

        [AllowAnonymous]
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
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            //public override void ExecuteResult(ControllerContext context)
            //{
            //    var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            //    if (UserId != null)
            //    {
            //        properties.Dictionary[XsrfKey] = UserId;
            //    }
            //    context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            //}
        }
        #endregion

        #region ApiLoginProvider
        //[HttpGet]
        //public void CreateUserProfile(string UserId, string Name, string EmailId)
        //{
        //    ConsumerMandetBo consBo = new ConsumerMandetBo();
        //    string firstName = null, lastName = null;
        //    string[] strArray = Name.Split(' ');
        //    for (int i = 0; i < strArray.Length; i = i + 2)
        //    {
        //        firstName = strArray[i];
        //        lastName = strArray[i];
        //    }

        //    consBo.Cons_First_Name = firstName;
        //    consBo.Cons_Last_Name = Name;
        //    consBo.Cons_mailid = EmailId;
        //    consBo.Cons_Pswd = UserId;
        //    consBo.Cons_Mobile = "";
        //    try
        //    {
        //        DataSet ds = _consumerService.AddConsumerMandet(consBo);
        //        int isTrue = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        //        if (isTrue != -1)
        //        {
        //            ConsumerLoginViewModel viewModel = new ConsumerLoginViewModel();
        //            viewModel.Cons_mailid = EmailId;
        //            viewModel.Cons_Pswd = UserId;
        //            LoginProvider(viewModel, null);
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);

        //    }
        //}

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginProvider(CorporateLoginViewModel corporateloginViewmodel, string returnUrl)
        {
            try
            {
                var loginBo = BuiltCorporateLoginBo(corporateloginViewmodel);
                DataSet data = _corporateService.FbCorporateLogin(loginBo);
                string user_Id = string.Empty;
                if (data.Tables.Count > 0)
                {
                    user_Id = data.Tables[0].Rows[0]["Cons_mailid"].ToString();

                    if (user_Id != "0")
                    {
                        System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            if (!string.IsNullOrEmpty(returnUrlProperty))
                            {
                                returnUrl = returnUrlProperty;
                            }
                        }
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                        }

                        return RedirectToAction("my_account", "Consumer");
                    }
                    else
                    {

                        return View(corporateloginViewmodel);
                    }

                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(corporateloginViewmodel);
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return RedirectToAction("Signin");
            }
        }

        public string WebLoginProvider(CorporateLoginViewModel corporateloginViewmodel, string returnUrl)
        {
            try
            {
                var loginBo = BuiltCorporateLoginBo(corporateloginViewmodel);
                DataSet data = _corporateService.CorporateLogin(loginBo);
                string user_Id = string.Empty;
                if (data.Tables.Count > 0)
                {
                    user_Id = data.Tables[0].Rows[0]["Cons_mailid"].ToString();

                    if (user_Id != "0")
                    {
                        System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                        return user_Id;
                    }
                    else
                    {
                        return user_Id;
                    }
                }
                else
                {
                    return user_Id;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return "0";
            }
        }
        #endregion
    }
}