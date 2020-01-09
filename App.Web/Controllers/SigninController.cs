using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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

    [ValidateInput(false)]
    public class SigninController : Controller
    {
        // GET: Signin
        public static string returnUrlProperty { get; set; }

        readonly IConsumerService _consumerService;

        public SigninController(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        //Consumer Account Login System

        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ConsumerLoginViewModel consumerloginViewmodel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(consumerloginViewmodel);
            }
            try
            {
                var loginBo = BuiltConsumerLoginBo(consumerloginViewmodel);
                DataSet data = _consumerService.ConsumerLogin(loginBo);
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

                        return RedirectToAction("Index", "my_account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(consumerloginViewmodel);
                    }

                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(consumerloginViewmodel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(consumerloginViewmodel);
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

        public ActionResult Logoff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            //HttpContext.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogoffCorporate()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            //HttpContext.SignOut();
            return RedirectToAction("SignIn", "Corporate");
        }

        public string GetLoggedInUserId()
        {
            return User.Identity.Name;
        }

        [AllowAnonymous]
        public ActionResult Forgot_Password()
        {
            return View();
        }

        private ConsumerLoginBo BuiltConsumerLoginBo(ConsumerLoginViewModel loginVm)
        {
            return (ConsumerLoginBo)new ConsumerLoginBo().InjectFrom(loginVm);
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
        [HttpGet]
        public void CreateUserProfile(string UserId, string Name, string EmailId)
        {
            ConsumerMandetBo consBo = new ConsumerMandetBo();
            string firstName = null, lastName = null;
            string[] strArray = Name.Split(' ');
            for (int i = 0; i < strArray.Length; i = i + 2)
            {
                firstName = strArray[i];
                lastName = strArray[i];
            }

            consBo.Cons_First_Name = firstName;
            consBo.Cons_Last_Name = Name;
            consBo.Cons_mailid = EmailId;
            consBo.Cons_Pswd = UserId;
            consBo.Cons_Mobile = "";
            try
            {
                DataSet ds = _consumerService.AddConsumerMandet(consBo);
                int isTrue = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                if (isTrue != -1)
                {
                    ConsumerLoginViewModel viewModel = new ConsumerLoginViewModel();
                    viewModel.Cons_mailid = EmailId;
                    viewModel.Cons_Pswd = UserId;
                    LoginProvider(viewModel, null);
                }

            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginProvider(ConsumerLoginViewModel consumerloginViewmodel, string returnUrl)
        {
            try
            {
                var loginBo = BuiltConsumerLoginBo(consumerloginViewmodel);
                DataSet data = _consumerService.ConsumerLogin(loginBo);
                string user_Id = string.Empty;
                if (data.Tables.Count > 0)
                {
                    user_Id = data.Tables[0].Rows[0]["Cons_Id"].ToString();

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

                        return View(consumerloginViewmodel);
                    }

                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(consumerloginViewmodel);
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return RedirectToAction("Signin");
            }
        }

        public string WebLoginProvider(ConsumerLoginViewModel consumerloginViewmodel, string returnUrl)
        {
            try
            {
                var loginBo = BuiltConsumerLoginBo(consumerloginViewmodel);
                DataSet data = _consumerService.ConsumerLogin(loginBo);
                string user_Id = string.Empty;
                if (data.Tables.Count > 0)
                {
                    user_Id = data.Tables[0].Rows[0]["Cons_Id"].ToString();

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