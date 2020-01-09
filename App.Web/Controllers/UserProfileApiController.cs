using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Util;
using App.BusinessObject;
using App.Common;
using App.UIServices;
using App.Web.ModelValidation;
using App.Web.ViewModels;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using App.DataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using System.Text;
using App.Domain;
using App.UIServices.InterfaceServices;
using System.Net.Mail;

namespace App.Web.Controllers
{
    [RoutePrefix("api/UserParam")]
    public class UserProfileApiController : ApiController
    {
        readonly IUserProfileServices _userprofileService;
        readonly IParamServices _paramServices;
        readonly ILoginServices _loginService;
        readonly IVendorService _vedorservices;

        public UserProfileApiController(IUserProfileServices userprofileService, IParamServices paramserv, ILoginServices loginService, IVendorService vendorservice)
        {
            _userprofileService = userprofileService;
            _paramServices = paramserv;
            _loginService = loginService;
            _vedorservices = vendorservice;
        }

        #region UserProfile
        [HttpPost("create")]
        public HttpResponseMessage AddUserProfile(UserProfileViewModel userprofileVm)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            //TransactionStatus transactionStatus;

            var results = new UserProfileValidation().Validate(userprofileVm);
            if (!results.IsValid)
            {
                userprofileVm.Errors = GenerateErrorMessage.Built(results.Errors);
                userprofileVm.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                userprofileVm.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, userprofileVm);
                return badResponse;
            }
            try
            {
                var userprofile = BuiltUserProfileBo(userprofileVm);

                DataSet Users = _userprofileService.AddUserProfile(userprofile);
                if (Users.Tables.Count <= 0)
                {
                    // TransactionStatus transactionStatus = new TransactionStatus(); 
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                    if (userprofileVm.Authority_Id == 4)
                    {
                        transactionStatus.ReturnMessage.Add("User Already Exist with this Email Id");
                    }
                    else
                    {
                        transactionStatus.ReturnMessage.Add("User Already Registered");
                    }
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, transactionStatus);
                    return badResponse;
                    //
                }
                else
                {

                    var jsonResult = JsonConvert.SerializeObject(Users);
                    if (jsonResult != null)
                    {
                        bool IsSent=false;
                        if (userprofileVm.Authority_Id == 3)
                        {
                            IsSent = SendEmail("LastMinuteKeys.com Vendor Portal Credential", "<br><br><div style='width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:'segoe UI';'><div style='background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;'><a href='#' style='display:inline-block;'><img src='http://www.lastminutekeys.com/img/Mailer/logo.png' title='Last Minute Keys' style='display:block;' /></a> </div><p style='color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);'> Dear <b>" +userprofile.Lastname + "</b>, <br />Thank you for partnering with LastMinuteKeys.<br />Please find below your Master Login details to access our Vendor system :<br/> URL : http://www.lastminutekeys.com/vendor/login <br /><br />Username :  " + userprofile.User_Name + " <br />Password :" + userprofile.Pswd + " <br/><br/>Regards,<br /> <b>The Lastminutekeys Team</b><br/> www.lastminutekeys.com</p></div></div>", userprofile.User_Name);
                        }
                        else
                        {
                             IsSent = SendEmail("Registration confirmation", "Dear " + userprofile.Firstname + ", <br/><br/>You have successfully set up your Lastminutekeys.com account<br/><br>Your registered email address is " + userprofile.User_Name + "<br/><br/>Lastminutekeys is a one stop shop mobile & web based solution to get you quality rooms at the last minute with the best price available. <br/><br/><br/>If you have any questions about our services, please go through our FAQ's <br/><br/><br/>Look forward in making your last minute booking experience a memorable one.<br/>Thank you for choosing Lastminutekeys<br/><br/> The Lastminutekeys Team<br/>", userprofile.User_Name);
                        }
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                        return response;
                    }
                    return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);

                }

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpPost("update")]
        public HttpResponseMessage UpdateUserProfile(UserProfileViewModel userprofileVm)
        {
            TransactionStatus transactionStatus;
            var results = new UserProfileValidationupdate().Validate(userprofileVm);
            if (!results.IsValid)
            {
                userprofileVm.Errors = GenerateErrorMessage.Built(results.Errors);
                userprofileVm.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                userprofileVm.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, userprofileVm);
                return badResponse;
            }
            try
            {
                var userprofile = BuiltUserProfileBo(userprofileVm);
                transactionStatus = _userprofileService.UpdateUserProfile(userprofile);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(userprofileVm));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Update Successfully.");
                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }
        [HttpGet("GetUserPermissions")]
        public DataSet GetUserPermissions()
        {

            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@User_Id",User.Identity.Name),//0
                 //new SqlParameter("@term",Url),//0
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectSuperAdminPagePermision", Params);

            return ds;
        }

        [HttpGet("Bind")]
        public HttpResponseMessage GetUserProfile(string AuthId, string UserId,string PropId)
        {
            var jsonResult = JsonConvert.SerializeObject(_userprofileService.GetUserProfile(AuthId, UserId, PropId));
            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        }

        [HttpPost("suspend")]
        public HttpResponseMessage SuspendUserProfile(UserProfileViewModel userprofileVm)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _userprofileService.SuspendUserProfile(userprofileVm.User_Id);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Warning.ToString();
                    transactionStatus.ReturnMessage.Add("Loyalty Not Suspended");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Loyalty successfully Suspended");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpGet("GetUserType")]
        public HttpResponseMessage GetUserType()
        {
            var jsonResult = JsonConvert.SerializeObject(_userprofileService.GetProfileMaster());
            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        }

        public UserProfileBo BuiltUserProfileBo(UserProfileViewModel userprofileVm)
        {
            return (UserProfileBo)new UserProfileBo().InjectFrom(userprofileVm);
        }
        #endregion

        #region Param
        [HttpPost("addParam")]
        public HttpResponseMessage AddParam(ParamViewModel paramVm)
        {
            var results = new ParamValidation().Validate(paramVm);
            if (!results.IsValid)
            {
                paramVm.Errors = GenerateErrorMessage.Built(results.Errors);
                paramVm.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                paramVm.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, paramVm);
                return badResponse;
            }
            try
            {
                var param = BuiltParamBo(paramVm);
                var jsonResult = JsonConvert.SerializeObject(_paramServices.AddParam(param));
                if (jsonResult != null)
                {
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                    return response;
                }
                return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("UpdateParam")]
        public HttpResponseMessage UpdateParam(ParamViewModel paramVm)
        {
            TransactionStatus transactionStatus;
            var results = new ParamValidation().Validate(paramVm);
            if (!results.IsValid)
            {
                paramVm.Errors = GenerateErrorMessage.Built(results.Errors);
                paramVm.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                paramVm.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, paramVm);
                return badResponse;
            }
            try
            {
                var param = BuiltParamBo(paramVm);
                transactionStatus = _paramServices.UpdateParam(param);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(paramVm));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("You are Successfully.");
                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpGet("GetParam")]
        public HttpResponseMessage GetParametersById(int propId, int authId)
        {
            var jsonResult = JsonConvert.SerializeObject(_paramServices.GetParam(propId, authId));
            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        }


        [HttpGet("UpdateParamType")]
        public HttpResponseMessage UpdateParamType(int Id, string type)
        {
            TransactionStatus transactionStatus;

            try
            {
                transactionStatus = _paramServices.UpdateParamType(Id, type);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString();
                    transactionStatus.ReturnMessage.Add("Not upadated");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus.ReturnMessage);

                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully upadated to database");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpGet("UpdateParamValue")]
        public HttpResponseMessage UpdateParamValue(int Id, string value)
        {
            TransactionStatus transactionStatus;

            try
            {
                transactionStatus = _paramServices.UpdateParamValue(Id, value);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString();
                    transactionStatus.ReturnMessage.Add("Not upadated");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus.ReturnMessage);

                    return badResponse;
                }
                else
                {
                    try
                    {
                        _vedorservices.ExecuteAddRoomTimer();  // start the Room inventory Timer
                    }
                    catch (Exception exc)
                    { ApplicationErrorLogServices.AppException(exc); }

                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Recorde successfully upadated to database");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpGet("UpdateParamHidden")]
        public HttpResponseMessage UpdateParamHidden(int Id, string value)
        {
            TransactionStatus transactionStatus;

            try
            {
                transactionStatus = _paramServices.UpdateParamHiddenGems(Id, value);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString();
                    transactionStatus.ReturnMessage.Add("Not upadated");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus.ReturnMessage);

                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Recorde successfully upadated to database");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpGet("UpdateParamPermission")]
        public HttpResponseMessage UpdateParamPermission(int Id, string flag)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _paramServices.UpdateParamPermission(Id, flag);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString();
                    transactionStatus.ReturnMessage.Add("Not upadated");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus.ReturnMessage);

                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Recorde successfully upadated to database");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("suspendParam")]
        public HttpResponseMessage SuspendParam(ParamViewModel paramVm)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _paramServices.SuspendParam(paramVm.Id);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Warning.ToString();
                    transactionStatus.ReturnMessage.Add("Param Not Suspended");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Param successfully Suspended");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        public ParamBo BuiltParamBo(ParamViewModel paramVm)
        {
            return (ParamBo)new ParamBo().InjectFrom(paramVm);
        }
        #endregion

        #region Permission
        [HttpGet("GetPermission")]
        public HttpResponseMessage GetPermission(int authId, int userId)
        {
            var jsonResult = JsonConvert.SerializeObject(_userprofileService.GetUserPermission(authId, userId));
            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        }

        [HttpGet("GetSuperAdminsPermission")]
        public HttpResponseMessage GetSuperAdminsPermission(int authId, int userId)
        {
            var jsonResult = JsonConvert.SerializeObject(_userprofileService.GetSuperAdminsPermission(authId, userId));
            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        }
        [HttpGet("UpdatePermission")]
        public HttpResponseMessage UpdateFlag(int userId, int pageId, string flag)
        {
            TransactionStatus transactionStatus;
            try
            {

                transactionStatus = _userprofileService.UpdatePermissionFlag(userId, pageId, flag);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString();
                    transactionStatus.ReturnMessage.Add("Not upadated");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus.ReturnMessage);

                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Successfully Updated");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }
        [HttpGet("UpdateSuperAdminPermissionFlag")]
        public HttpResponseMessage UpdateSuperAdminPermissionFlag(int userId, int pageId, string flag)
        {
            TransactionStatus transactionStatus;
            try
            {

                transactionStatus = _userprofileService.UpdateSuperAdminPermissionFlag(userId, pageId, flag);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString();
                    transactionStatus.ReturnMessage.Add("Not upadated");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus.ReturnMessage);

                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Successfully Updated");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }
        #endregion




        public bool SendEmail(string subject, string body, string reciver)
        {
            try
            {
                EmailMaster emailmaster = _loginService.EmailCredentials();

                string senderID = emailmaster.Email;
                string senderPassword = emailmaster.Password;
                SmtpClient smtp = new SmtpClient(emailmaster.SMTP);
                smtp.Port = emailmaster.Port;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = true;
                System.Net.NetworkCredential credential = new System.Net.NetworkCredential(senderID, senderPassword);  //smtp.EnableSsl = ObjEmail.EnableSsl;
                smtp.Credentials = credential;

                var message = new MailMessage(senderID, reciver);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                smtp.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return false;
            }

        }

    }
}