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
using System.Security.Cryptography;
using System.IO;

namespace App.Web.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginApiController : ApiController
    {
        const string DESKey = "AQWSEDRF";
        const string DESIV = "HGFEDCBA";
        readonly ILoginServices _loginService;

        public LoginApiController(ILoginServices loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("signin")]
        public HttpResponseMessage SigninUser(LoginViewModel loginVM)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            var results = new LoginValidation().Validate(loginVM);
            if (!results.IsValid)
            {
                loginVM.Errors = GenerateErrorMessage.Built(results.Errors);
                loginVM.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                loginVM.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, loginVM);
                return badResponse;
            }
            try
            {
                var login = BuiltLoginBo(loginVM);
                List<string> lst = _loginService.AuthenticateUser(login);
                if (lst.Count > 0)
                    transactionStatus.Status = true;
                else
                    transactionStatus.Status = false;
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Not Valid User ! Please SignUp First");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Success");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                transactionStatus.ReturnMessage.Add("Exception Occurred :" + ex.Message.ToString());
                var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                return badResponse;
            }
        }

        [HttpPost("forgot")]
        public HttpResponseMessage ForgotPassword(ForgotLinkViewModel linkVm)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            var results = new ForgotLinkValidation().Validate(linkVm);
            if (!results.IsValid)
            {
                linkVm.Errors = GenerateErrorMessage.Built(results.Errors);
                linkVm.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                linkVm.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, linkVm);
                return badResponse;
            }
            try
            {
                var link = BuiltForgotBo(linkVm);

                ForgotPassword userValue = _loginService.FindUserByEmail(link);
                if (string.IsNullOrEmpty(userValue.Email) != true)
                {

                    var callbackUrl = "This email was sent in response to your request to change your Lastminutekeys password.<br/><br/>To reset your password, click on the link below. For security reasons, this link will only remain active for the next 24 hours.<br/><br/><a href=http://www.lastminutekeys.com/Login/CreatePassword/" +userValue.Id.ToString() + ">Change your password</a><br/><br/>Please do not reply to this email.<br/><br/>Thank you for using Lastminutekeys.com ";
                    bool IsSent = SendEmail("Your Request to Change Lastminutekeys password",  callbackUrl, userValue.Email);
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    if (IsSent)
                        transactionStatus.ReturnMessage.Add("Email Sent Successfully. Please Check Email");
                    else
                        transactionStatus.ReturnMessage.Add("Email Not Sent. Please Retry.");
                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower(); 
                    transactionStatus.ReturnMessage.Add("Not a Registered Email Id");
                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return badResponse;
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                transactionStatus.ReturnMessage.Add("Exception Occurred :" + ex.Message.ToString());
                var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                return badResponse;
            }
        }

       
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

   
        private LoginBo BuiltLoginBo(LoginViewModel loginVm)
        {
            return (LoginBo)new LoginBo().InjectFrom(loginVm);
        }

        private ForgotPasswordBo BuiltForgotBo(ForgotLinkViewModel link)
        {
            return (ForgotPasswordBo)new ForgotPasswordBo().InjectFrom(link);
        }

        #region CreatePassword

        [HttpPost("createpassword")]
        public HttpResponseMessage CreatePassword(ResetPasswordViewModel resetpasswordVM)
        {
            var results = new ResetPasswordValidation().Validate(resetpasswordVM);
            if (!results.IsValid)
            {
                resetpasswordVM.Errors = GenerateErrorMessage.Built(results.Errors);
                resetpasswordVM.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                resetpasswordVM.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, resetpasswordVM);
                return badResponse;
            }
            TransactionStatus transactionStatus;
            try
            {
                var passwordBo = BuiltResetPasswordBo(resetpasswordVM);
                transactionStatus = _loginService.ResetPassword(passwordBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(resetpasswordVM));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Password Created Successfully.");
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

        public ResetPasswordBo BuiltResetPasswordBo(ResetPasswordViewModel paramVm)
        {
            return (ResetPasswordBo)new ResetPasswordBo().InjectFrom(paramVm);
        }

        #endregion

        public static string DESEncrypt(string stringToEncrypt)// Encrypt the content
        {


            byte[] key;
            byte[] IV;


            byte[] inputByteArray;
            try
            {


                key = Convert2ByteArray(DESKey);


                IV = Convert2ByteArray(DESIV);


                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();


                MemoryStream ms = new MemoryStream(); CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);


                cs.FlushFinalBlock();


                return Convert.ToBase64String(ms.ToArray());
            }


            catch (System.Exception ex)
            {


                throw ex;
            }


        }


        public static string DESDecrypt(string stringToDecrypt)//Decrypt the content
        {


            byte[] key;
            byte[] IV;


            byte[] inputByteArray;
            try
            {


                key = Convert2ByteArray(DESKey);


                IV = Convert2ByteArray(DESIV);


                int len = stringToDecrypt.Length; inputByteArray = Convert.FromBase64String(stringToDecrypt);




                DESCryptoServiceProvider des = new DESCryptoServiceProvider();


                MemoryStream ms = new MemoryStream(); CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);


                cs.FlushFinalBlock();


                Encoding encoding = Encoding.UTF8; return encoding.GetString(ms.ToArray());
            }


            catch (System.Exception ex)
            {


                throw ex;
            }










        }
        static byte[] Convert2ByteArray(string strInput)
        {


            int intCounter; char[] arrChar;
            arrChar = strInput.ToCharArray();


            byte[] arrByte = new byte[arrChar.Length];


            for (intCounter = 0; intCounter <= arrByte.Length - 1; intCounter++)
                arrByte[intCounter] = Convert.ToByte(arrChar[intCounter]);


            return arrByte;
        }
    }
}