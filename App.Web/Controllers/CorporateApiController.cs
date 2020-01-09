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
using CCA.Util;

namespace App.Web.Controllers
{
    [RoutePrefix("api/Corporate")]
    public class CorporateApiController : ApiController
    {
        CCACrypto ccaCrypto = new CCACrypto();
        string workingKey = "817C1DFAD9D36A924621C42B79CD4C0E";//put in the 32bit alpha numeric key in the quotes provided here 	
        string ccaRequest = "";
        public string strEncRequest = "";
        public string strAccessCode = "AVMF04CA21BU64FMUB";// put the access key in the quotes provided here.
        readonly ICorporateService _corporateService;
        readonly ILoginServices _loginService;

        public CorporateApiController(ICorporateService corporateservice, ILoginServices loginservice)
        {
            _corporateService = corporateservice;
            _loginService = loginservice;
        }
        //Create Consumer with all fields

        [System.Web.Http.HttpPost("createForAllFields")]
        //public HttpResponseMessage CreateConsumer(CorporateViewModel corporateViewModel)
        //{
        //    TransactionStatus transactionStatus;
        //    var results = new CorporateValidation().Validate(corporateViewModel);

        //    if (!results.IsValid)
        //    {
        //        corporateViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        corporateViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        corporateViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, corporateViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var consBo = BuiltCorporateBo(corporateViewModel);
        //        transactionStatus = _corporateService.AddConsumer(consBo);

        //        if (transactionStatus.Status == false)
        //        {
        //            var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(corporateViewModel));
        //            return badResponse;
        //        }
        //        else
        //        {
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            transactionStatus.ReturnMessage.Add("Record successfully inserted to database");

        //            var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

        //            return badResponse;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}
        //[System.Web.Http.HttpPost("createMandetory")]
        //public HttpResponseMessage CreateConsumerMandet(ConsumerMandetViewModel consumerViewModel)
        //{
        //    TransactionStatus transactionStatus = new TransactionStatus();
        //    var results = new ConsumerMandetValidation().Validate(consumerViewModel);

        //    if (!results.IsValid)
        //    {
        //        consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        consumerViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var consBo = BuiltConsumerMandetBo(consumerViewModel);

        //        DataSet dsUserDetails;
        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.AddConsumerMandet(consBo));
        //        //dsUserDetails = _consumerService.AddConsumerMandet(consBo);
        //        //if (dsUserDetails.Tables[0].Rows[0][0].ToString() != "10")
        //        //{
        //        //    bool IsSent = SendEmails("Mail From LMK", "Welcome to LMK <b>", consBo.Cons_mailid.ToString());
        //        //}



        //        var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //        response.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");

        //        return response;

        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}


        [System.Web.Http.HttpPost("corporateLogin")]
        public HttpResponseMessage ConsumerLogin(CorporateLoginViewModel corporateloginViewModel)
        {
            List<Object> objUserId = new List<object>();


            var results = new CorporateLoginValidation().Validate(corporateloginViewModel);

            if (!results.IsValid)
            {
                objUserId.Add("required");
                var jsonResult = JsonConvert.SerializeObject(objUserId);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            try
            {
                var consBo = BuiltCorporateLoginBo(corporateloginViewModel);

                var jsonResultdata = JsonConvert.SerializeObject(_corporateService.CorporateLogin(consBo));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                //{"City_Id":1,"City_Name":"abc"}
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        //[System.Web.Http.HttpPost("consumerDetails")]
        //public HttpResponseMessage ConsumerDetails(ConsumerDetailsViewModel consumerDetailsViewModel)
        //{
        //    List<Object> objUserId = new List<object>();

        //    DataSet dsUserDetails;
        //    var results = new ConsumerDetailsValidation().Validate(consumerDetailsViewModel);

        //    if (!results.IsValid)
        //    {
        //        objUserId.Add("required");
        //        var jsonResult = JsonConvert.SerializeObject(objUserId);
        //        var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //        response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //        return response;
        //    }
        //    try
        //    {
        //        var consBo = BuiltConsumerDetailsBo(consumerDetailsViewModel);
        //        dsUserDetails = _corporateService.ConsumerDetails(consBo);

        //        objUserId.Add(dsUserDetails.Tables[0].Rows[0][0].ToString());

        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.ConsumerDetails(consBo));
        //        var response1 = this.Request.CreateResponse(HttpStatusCode.OK);
        //        response1.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return response1;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[System.Web.Http.HttpPost("consumerForgotPassword")]
        //public HttpResponseMessage ConsumerForgotPassword(ConsumerForgotpwdViewModel consumerforgotpwdViewModel)
        //{
        //    TransactionStatus transactionStatus = new TransactionStatus();
        //    List<Object> objUserpwd = new List<object>();

        //    DataSet dsUserDetails;
        //    var results = new ConsumerForgotpwdValidation().Validate(consumerforgotpwdViewModel);

        //    if (!results.IsValid)
        //    {
        //        objUserpwd.Add("required");
        //        var jsonResult = JsonConvert.SerializeObject(objUserpwd);
        //        var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //        response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //        return response;
        //    }
        //    try
        //    {
        //        var consBo = BuiltConsumerForgotpwdBo(consumerforgotpwdViewModel);
        //        dsUserDetails = _corporateService.ConsumerForgotpwd(consBo);

        //        objUserpwd.Add(dsUserDetails.Tables[0].Rows[0][0].ToString());
        //        if (dsUserDetails.Tables[0].Rows[0][0].ToString() != "not exists")
        //        {
        //            var html = "<div style='width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:'segoe UI';'><div style='background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;'><a href='#' style='display:inline-block;'><img src='http://www.lastminutekeys.com/img/Mailer/logo.png' title='Last Minute Keys' style='display:block;' /></a> </div><p style='color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);'> Your  Lastminutekeys.com account password is : <b>" + dsUserDetails.Tables[0].Rows[0][0].ToString() + "</b> <br /> <br /><b>The Lastminutekeys Team</b><br/> www.lastminutekeys.com</p><div style='background:#192b3e;padding:10px;'><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;'>Follow Us</h4><ul style='padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;'><li style='float:left;width:33.3%;text-align:left;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;'></i> Follow us on Twitter</a></li><li style='float:left;width:33.3%;text-align:center;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Facebook</a></li><li style='float:left;width:33.3%;text-align:right;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Instagram</a></li><div style='clear:both;'></div></ul><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;'>Contact Our Customer Care</h4><p style='color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;'><a href='www.lastminutekeys.com/Home/Contact_Us' style='color:#FFF;font-size:14px;font-weight:400;'>Click here</a></p></div></div>";
        //            bool IsSent = SendEmails("Mail From LMK", html, consBo.Cons_mailid.ToString());
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            if (IsSent)
        //                transactionStatus.ReturnMessage.Add("Email Sent Successfully. Please Check Email");
        //            else
        //                transactionStatus.ReturnMessage.Add("Email Not Sent. Please Retry.");
        //            //Admin
        //            IsSent = SendEmails("Mail From LMK", html, "info@lastminutekeys.com");
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            if (IsSent)
        //                transactionStatus.ReturnMessage.Add("Email Sent Successfully. Please Check Email");
        //            else
        //                transactionStatus.ReturnMessage.Add("Email Not Sent. Please Retry.");
                    
        //        }
        //        else
        //        {
        //            transactionStatus.ReturnMessage.Add("Mail doesn't Exist");
        //        }
        //        var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

        //        return badResponse;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}
        ////Mail method
        //public bool SendEmails(string subject, string body, string reciver)
        //{
        //    try
        //    {
        //        EmailMaster emailmaster = _loginService.EmailCredentials();

        //        string senderID = emailmaster.Email;
        //        string senderPassword = emailmaster.Password;
        //        SmtpClient smtp = new SmtpClient(emailmaster.SMTP);
        //        smtp.Port = emailmaster.Port;
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.UseDefaultCredentials = true;
        //        System.Net.NetworkCredential credential = new System.Net.NetworkCredential(senderID, senderPassword);  //smtp.EnableSsl = ObjEmail.EnableSsl;
        //        smtp.Credentials = credential;

        //        var message = new MailMessage(senderID, reciver);
        //        message.Subject = subject;
        //        message.Body = body;
        //        message.IsBodyHtml = true;

        //        smtp.Send(message);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return false;
        //    }

        //}
        ////for Update Consumer

        //[System.Web.Http.HttpPost("UpdateConsumer")]
        //public HttpResponseMessage UpdateConsumer(ConsumerViewModel consumerViewModel)
        //{
        //    TransactionStatus transactionStatus;
        //    var results = new ConsumerValidation().Validate(consumerViewModel);

        //    if (!results.IsValid)
        //    {
        //        consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        consumerViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var consBo = BuiltCorporateBo(consumerViewModel);
        //        transactionStatus = _corporateService.UpdateConsumer(consBo);

        //        if (transactionStatus.Status == false)
        //        {
        //            var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(consumerViewModel));
        //            return badResponse;
        //        }
        //        else
        //        {
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            transactionStatus.ReturnMessage.Add("Profile successfully updated!!!");

        //            var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

        //            return badResponse;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}
        ////for reset paswd
        //[System.Web.Http.HttpPost("UpdateConsumerPswd")]
        //public HttpResponseMessage UpdateConsumerPswd(ConsumerViewModel consumerViewModel)
        //{
        //    TransactionStatus transactionStatus;
        //    var results = new ConsumerChagePasswordValidation1().Validate(consumerViewModel);

        //    if (!results.IsValid)
        //    {
        //        consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        consumerViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var consBo = BuiltCorporateBo(consumerViewModel);
        //        transactionStatus = _corporateService.UpdateConsumerPswd(consBo);

        //        if (transactionStatus.Status == false)
        //        {
        //            var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(consumerViewModel));
        //            return badResponse;
        //        }
        //        else
        //        {
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            transactionStatus.ReturnMessage.Add("Password updated!!");

        //            var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

        //            return badResponse;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[System.Web.Http.HttpPost("PropertyListing")]
        //public HttpResponseMessage PropertyList(PropertyViewModel propViewModel)
        //{

        //    List<object> objPropList = new List<object>();

        //    DataSet dsPropertyDetails;

        //    try
        //    {
        //        var consBo = BuiltPropBo(propViewModel);
        //        dsPropertyDetails = _corporateService.PropertyList(consBo);

        //        objPropList.Add(dsPropertyDetails);

        //        var jsonResultdata = JsonConvert.SerializeObject(objPropList);
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}


        ////lISTING WITH SORT

        //[System.Web.Http.HttpPost("PropertyListing_Sort")]
        //public HttpResponseMessage PropertyList_Sort(PropertyViewModel propViewModel)
        //{

        //    List<Object> objPropList = new List<object>();

        //    DataSet dsPropertyDetails;

        //    try
        //    {
        //        var consBo = BuiltPropBo(propViewModel);
        //        dsPropertyDetails = _corporateService.PropertyList_Sort(consBo);

        //        objPropList.Add(dsPropertyDetails);

        //        var jsonResultdata = JsonConvert.SerializeObject(objPropList);
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}
        //[System.Web.Http.HttpPost("PropertyListingDetails")]
        //public HttpResponseMessage PropertyListDetails(PropertyDetailsViewModel propViewModel)
        //{

        //    List<Object> objPropList = new List<object>();

        //    DataSet dsPropertyDetails;


        //    try
        //    {
        //        var consBo = BuiltPropDetailsBo(propViewModel);
        //        dsPropertyDetails = _corporateService.PropertyListDetails(consBo);

        //        objPropList.Add(dsPropertyDetails);

        //        var jsonResultdata = JsonConvert.SerializeObject(objPropList);
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        ////proc_Room_Listing
        //[System.Web.Http.HttpPost("Room_Listing")]
        //public HttpResponseMessage RoomList(PropertyDetailsViewModel1 propViewModel)
        //{
        //    List<Object> objPropList = new List<object>();

        //    DataSet dsPropertyDetails;


        //    try
        //    {

        //        var consBo = BuiltPropDetailsRoomBo(propViewModel);
        //        dsPropertyDetails = _corporateService.RoomList(consBo);

        //        objPropList.Add(dsPropertyDetails);

        //        var jsonResultdata = JsonConvert.SerializeObject(objPropList);
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[System.Web.Http.HttpGet("allcities")]
        //public HttpResponseMessage GetCity()
        //{
        //    var Citylist = _corporateService.GetCity();
        //    var badResponse = Request.CreateResponse(HttpStatusCode.OK, Citylist);
        //    return badResponse;

        //}

        //[System.Web.Http.HttpGet("alllocations")]
        //public HttpResponseMessage GetLocations()
        //{
        //    var Locatiolist = _corporateService.GetLocations();
        //    var badResponse = Request.CreateResponse(HttpStatusCode.OK, Locatiolist);
        //    return badResponse;
        //}
        //[System.Web.Http.HttpPost("SendMail")]
        //public HttpResponseMessage SendEmail(ConsumerFormViewModel cons)
        //{
        //    TransactionStatus transactionStatus = new TransactionStatus();
        //    try
        //    {

        //        EmailMaster emailmaster = _loginService.EmailCredentials();

        //        string senderID = emailmaster.Email;
        //        string senderPassword = emailmaster.Password;
        //        SmtpClient smtp = new SmtpClient(emailmaster.SMTP);
        //        smtp.Port = emailmaster.Port;
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.UseDefaultCredentials = true;
        //        System.Net.NetworkCredential credential = new System.Net.NetworkCredential(senderID, senderPassword);  //smtp.EnableSsl = ObjEmail.EnableSsl;
        //        smtp.Credentials = credential;

        //        var message = new MailMessage(senderID, cons.Cons_mailid);
        //        message.Subject = cons.Cons_Subject;
        //        message.Body = cons.Cons_Body;
        //        message.IsBodyHtml = true;

        //        smtp.Send(message);

        //        transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();

        //        transactionStatus.ReturnMessage.Add("Email Sent Successfully. Please Check Email");

        //        var goodresponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

        //        return goodresponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        transactionStatus.ReturnMessage.Add("Exception Occurred :" + ex.Message.ToString());
        //        var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
        //        return badResponse;
        //    }

        //}


        ////Prebooking

        //[System.Web.Http.HttpPost("PreBooking")]
        //public HttpResponseMessage PreBooking(PrebookingViewModel preViewModel)
        //{

        //    List<Object> BookingList = new List<object>();

        //    DataSet dsBookingDetails;

        //    try
        //    {
        //        var consBo = BuiltPreBo(preViewModel);
        //        dsBookingDetails = _corporateService.PreBooking(consBo);

        //        BookingList.Add(dsBookingDetails);

        //        var jsonResultdata = JsonConvert.SerializeObject(BookingList);
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}
        ////Cc Avenue
        //[System.Web.Http.HttpGet("CCAvenue")]
        //public string CCAvenue(string Invce_Num, double Amount)
        //{
        //    try
        //    {

        //        DataSet dsdata = new DataSet();
        //        dsdata = _corporateService.GetTransaction(Invce_Num);

        //        ccaRequest = ccaRequest + "merchant_id" + "=" + "56460&";


        //        ccaRequest = ccaRequest + "order_id" + "=" + Invce_Num + "&";
        //        double tot = Convert.ToDouble(Amount);
        //        ccaRequest = ccaRequest + "amount" + "=" + tot + "&";
        //        ccaRequest = ccaRequest + "currency" + "=" + "INR&";
        //        ccaRequest = ccaRequest + "redirect_url" + "=" + "http://www.lastminutekeys.com/response&";
        //        ccaRequest = ccaRequest + "cancel_url" + "=" + "http://www.lastminutekeys.com/Cancel&";

        //        //Billing Details
        //        //  dsdata = (DataSet)Session["raiaddrs"];
        //        //ccaRequest = ccaRequest + "billing_name" + "=" + dsdata.Tables[0].Rows[0]["name"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "billing_address" + "=" + dsdata.Tables[0].Rows[0]["address"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "billing_city" + "=" + dsdata.Tables[0].Rows[0]["city"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "billing_state" + "=" + dsdata.Tables[0].Rows[0]["state"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "billing_zip" + "=" + dsdata.Tables[0].Rows[0]["pin"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "billing_country" + "=" + "India&";
        //        //ccaRequest = ccaRequest + "billing_tel" + "=" + dsdata.Tables[0].Rows[0]["mob"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "billing_email" + "=" + dsdata.Tables[0].Rows[0]["email"].ToString() + "&";

        //        ccaRequest = ccaRequest + "billing_name" + "=" + dsdata.Tables[0].Rows[0]["Cons_First_Name"].ToString() + "&";
        //        ccaRequest = ccaRequest + "billing_address" + "=" + dsdata.Tables[0].Rows[0]["Cons_Addr1"].ToString() + dsdata.Tables[0].Rows[0]["Cons_Addr2"].ToString() + "&";
        //        ccaRequest = ccaRequest + "billing_city" + "=" + dsdata.Tables[0].Rows[0]["Cons_City"].ToString() + "&";
        //        ccaRequest = ccaRequest + "billing_state" + "=" + "NULL&";
        //        ccaRequest = ccaRequest + "billing_zip" + "=" + "NULL&";
        //        ccaRequest = ccaRequest + "billing_country" + "=" + "India&";
        //        ccaRequest = ccaRequest + "billing_tel" + "=" + dsdata.Tables[0].Rows[0]["Cons_Mobile"].ToString() + "&";
        //        ccaRequest = ccaRequest + "billing_email" + "=" + dsdata.Tables[0].Rows[0]["Cons_mailid"].ToString() + "&";
        //        //Shipment Details

        //        //ccaRequest = ccaRequest + "delivery_name" + "=" + dsdata.Tables[0].Rows[0]["name"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "delivery_address" + "=" + dsdata.Tables[0].Rows[0]["address"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "delivery_city" + "=" + dsdata.Tables[0].Rows[0]["city"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "delivery_state" + "=" + dsdata.Tables[0].Rows[0]["state"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "delivery_zip" + "=" + dsdata.Tables[0].Rows[0]["pin"].ToString() + "&";
        //        //ccaRequest = ccaRequest + "delivery_country" + "=" + "India&";
        //        //ccaRequest = ccaRequest + "delivery_tel" + "=" + dsdata.Tables[0].Rows[0]["mob"].ToString() + "&";
        //        ccaRequest = ccaRequest + "delivery_name" + "=" + "=" + "NULL&";
        //        ccaRequest = ccaRequest + "delivery_address" + "=" + "NULL&";
        //        ccaRequest = ccaRequest + "delivery_city" + "=" + "NULL&";
        //        ccaRequest = ccaRequest + "delivery_state" + "=" + "NULL&";
        //        ccaRequest = ccaRequest + "delivery_zip" + "=" + "NULL&";
        //        ccaRequest = ccaRequest + "delivery_country" + "=" + "India&";
        //        ccaRequest = ccaRequest + "delivery_tel" + "=" + "NULL&&";

        //        ccaRequest = ccaRequest + "merchant_param1" + "=" + "additional Info.&";
        //        ccaRequest = ccaRequest + "merchant_param2" + "=" + "additional Info.&";
        //        ccaRequest = ccaRequest + "merchant_param3" + "=" + "additional Info.&";
        //        ccaRequest = ccaRequest + "merchant_param4" + "=" + "additional Info.&";
        //        ccaRequest = ccaRequest + "merchant_param5" + "=" + "additional Info.&";
        //        ccaRequest = ccaRequest + " promo_code" + "=" + "&";
        //        ccaRequest = ccaRequest + "customer_identifier" + "=" + dsdata.Tables[0].Rows[0]["Cons_mailid"].ToString() + "&";

        //        return strEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);
        //    }


        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }


        //}

        //[System.Web.Http.HttpGet("Acess_Code")]
        //public string Access_Code()
        //{
        //    return strAccessCode;
        //}

        ////public HttpResponseMessage CCAvenueRespone()
        ////{
        ////    string workingKey = "817C1DFAD9D36A924621C42B79CD4C0E";//put in the 32bit alpha numeric key in the quotes provided here
        ////    CCACrypto ccaCrypto = new CCACrypto();
        ////    string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
        ////    Label1.Text = encResponse.ToString();
        ////    //NameValueCollection Params = new NameValueCollection();
        ////    string[] segments = encResponse.Split('&');
        ////    string[] ordrid = segments[0].Split('=');
        ////    string[] ordrstatus = segments[3].Split('=');
        ////    Label1.Text = segments[0] + " <br/> " + segments[3] + " <br/> " + segments[4] + " <br/> " + encResponse.ToString();
        ////    lblid.Value = ordrid[1];
        ////    lblstatus.Value = ordrstatus[1];


        ////}

        //[System.Web.Http.HttpPost("PreBookingUpdate")]
        //public HttpResponseMessage PreBookingUpdate(PrebookingViewModel preViewModel)
        //{

        //    List<Object> BookingList = new List<object>();

        //    DataSet dsBookingDetails;

        //    try
        //    {
        //        var consBo = BuiltPreBo(preViewModel);
        //        dsBookingDetails = _corporateService.PreBookingUpdate(consBo);

        //        BookingList.Add(dsBookingDetails);

        //        var jsonResultdata = JsonConvert.SerializeObject(BookingList);
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[System.Web.Http.HttpGet("GetTransaction")]
        //public HttpResponseMessage GetTransaction(string Invce_Num)
        //{

        //    try
        //    {
        //        var jsonResult = JsonConvert.SerializeObject(_corporateService.GetTransaction(Invce_Num));
        //        if (jsonResult != null)
        //        {
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //            return response;
        //        }
        //        return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}
        //[System.Web.Http.HttpPost("GetAllTransaction")]
        //public HttpResponseMessage GetAllTransaction(PrebookingViewModel preViewModel)
        //{

        //    List<Object> BookingList = new List<object>();

        //    DataSet dsBookingDetails;

        //    try
        //    {
        //        var consBo = BuiltPreBo(preViewModel);
        //        dsBookingDetails = _corporateService.GetAllTransaction(consBo);

        //        BookingList.Add(dsBookingDetails);

        //        var jsonResultdata = JsonConvert.SerializeObject(BookingList);
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //#region WebApi

        //[HttpPost("SignUpConsumer")]
        //public HttpResponseMessage SignUpConsumer(ConsumerMandetViewModel consumerViewModel)
        //{

        //    var results = new ConsumerSignUpValidation().Validate(consumerViewModel);

        //    if (!results.IsValid)
        //    {
        //        consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        consumerViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var consBo = BuiltConsumerMandetBo(consumerViewModel);

             

        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.AddConsumerMandet(consBo));
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);

        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;

        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        [HttpPost("WebLogin")]
        public HttpResponseMessage WebConsumerLogin(CorporateWebLoginViewModel loginViewModel)
        {
            var results = new CorporateWebLoginValidation().Validate(loginViewModel);

            if (!results.IsValid)
            {
                loginViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                loginViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                loginViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, loginViewModel);
                return badResponse;
            }
            try
            {
                string tempUrl = loginViewModel.returnUrl;

                CorporateLoginViewModel viewModel = new CorporateLoginViewModel();
                CorporateController ctrl = new CorporateController(_corporateService);
                viewModel.Corp_mailid = loginViewModel.Corp_mailid;
                viewModel.Corp_Pswd = loginViewModel.Corp_Pswd;
                string IsValiddate = ctrl.WebLoginProvider(viewModel, null);
                if (IsValiddate != "0")
                {
                    if (!string.IsNullOrEmpty(loginViewModel.returnUrl))
                    {
                        if (tempUrl.Contains("http://"))
                            loginViewModel.returnUrl = "Home";
                        if (tempUrl.Contains("/signin"))
                            loginViewModel.returnUrl = "my_account";
                        if (tempUrl.Contains("/Signin?ReturnUrl=%2fBooking_payment_method"))
                            loginViewModel.returnUrl = "Booking_payment_method";
                        if (tempUrl.Contains("/Search_Results"))
                            loginViewModel.returnUrl = "Search_Results";
                        if (tempUrl.Contains("/hotel_details"))
                            loginViewModel.returnUrl = "hotel_details";

                    }
                    else
                        loginViewModel.returnUrl = "my_account";
                    var jsonResultdata = JsonConvert.SerializeObject(loginViewModel.returnUrl);
                    var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                    responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                    return responsedata;
                }
                else
                {
                    var jsonResultdata = JsonConvert.SerializeObject(null);
                    var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                    responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                    return responsedata;
                }

            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //[HttpGet("AuthenticateUser")]
        //public HttpResponseMessage ExternalLogin(string UserId, string Name, string EmailId, string returnUrl)
        //{
        //    string[] str = null; string tempUrl = returnUrl;
        //    ConsumerMandetBo consBo = new ConsumerMandetBo();
        //    string firstName = null, lastName = string.Empty;
        //    string[] strArray = Name.Split(' ');
        //    if (strArray.Length > 0)
        //        firstName = strArray[0];
        //    for (int i = 0; i < strArray.Length; i++)
        //    {
        //        lastName = strArray[1];
        //    }
        //    consBo.Cons_First_Name = firstName;
        //    consBo.Cons_Last_Name = string.IsNullOrEmpty(lastName) ? Name : lastName;
        //    consBo.Cons_mailid = EmailId;
        //    consBo.Cons_Pswd = UserId;

        //    try
        //    {
        //        DataSet ds = _corporateService.AddConsumerMandet(consBo);
        //        int isTrue = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        //        if (isTrue != -1)
        //        {
        //            ConsumerLoginViewModel viewModel = new ConsumerLoginViewModel();
        //            ConsumerController ctrl = new ConsumerController(_corporateService);
        //            viewModel.Cons_mailid = EmailId;
        //            viewModel.Cons_Pswd = UserId;
        //            ctrl.LoginProvider(viewModel, null);
        //            if (!string.IsNullOrEmpty(returnUrl))
        //            {
        //                //if (tempUrl.Contains("http://"))
        //                //    returnUrl = "";
        //                if (tempUrl.Contains("/home"))
        //                    returnUrl = "Home";
        //                if (tempUrl.Contains("/home"))
        //                    returnUrl = "/Home";
        //                if (tempUrl.Contains("/Signin"))
        //                    returnUrl = "/my_account";
        //                if (tempUrl.Contains("Booking_payment_method"))
        //                    returnUrl = "/Booking_payment_method";
        //                if (tempUrl.Contains("/Search_Results"))
        //                    returnUrl = "Search_Results";
        //                if (tempUrl.Contains("/hotel_details"))
        //                    returnUrl = "/hotel_details";

        //            }
        //            else
        //                returnUrl = "my_account";
        //            var jsonResultdata = JsonConvert.SerializeObject(returnUrl);
        //            var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //            responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //            return responsedata;
        //        }
        //        return null;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        //[HttpPost("UpdateConsumerProfile")]
        //public HttpResponseMessage UpdateConsumerProfile(ConsumerFormViewModel consumerViewModel)
        //{
        //    TransactionStatus transactionStatus;
        //    var results = new ConsumerUpdateProfileValidation().Validate(consumerViewModel);

        //    if (!results.IsValid)
        //    {
        //        consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        consumerViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var consBo = BuiltConsumerFormBo(consumerViewModel);
        //        transactionStatus = _corporateService.UpdateConsumerProfile(consBo);

        //        if (transactionStatus.Status == false)
        //        {
        //            var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(consumerViewModel));
        //            return badResponse;
        //        }
        //        else
        //        {
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            transactionStatus.ReturnMessage.Add("Profile successfully updated!!!");

        //            var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

        //            return badResponse;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[HttpGet("GetBookedOverviewDeals")]
        //public HttpResponseMessage GetBookedOverviewTransactionById(string Cons_Id)
        //{
        //    try
        //    {
        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.GetOverviewBookedDealsById(Cons_Id));
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[HttpGet("GetBookedDeals")]
        //public HttpResponseMessage GetBookedTransactionById(string Cons_Id)
        //{
        //    try
        //    {
        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.GetBookedTransactionById(Cons_Id));
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        [HttpGet("GetProfileDetails")]
        public HttpResponseMessage GetProfileDetails(string Cons_Id)
        {
            try
            {
                var ConsuBo = new ConsumerDetailsBo();
                //  ConsuBo.Cons_Id = Cons_Id;

                var jsonResultdata = JsonConvert.SerializeObject(_corporateService.GetProfileDetails(Cons_Id));
                var response1 = this.Request.CreateResponse(HttpStatusCode.OK);
                response1.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return response1;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        //[HttpPost("ChangePassword")]
        //public HttpResponseMessage ChangePassword(ConsumerChangePasswordViewModel changepasswordViewModel)
        //{
        //    TransactionStatus transactionStatus;
        //    var results = new ConsumerChagePasswordValidation().Validate(changepasswordViewModel);

        //    if (!results.IsValid)
        //    {
        //        changepasswordViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        changepasswordViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        changepasswordViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, changepasswordViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var consBo = BuiltConsumerChangePasswordBo(changepasswordViewModel);
        //        transactionStatus = _corporateService.ChangePassword(consBo);

        //        if (transactionStatus.Status == false)
        //        {
        //            transactionStatus.Status = true;
        //            transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //            transactionStatus.ReturnMessage.Add("Current Password Is Wrong! Please Retry");

        //            var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, transactionStatus);
        //            return badResponse;
        //        }
        //        else
        //        {
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            transactionStatus.ReturnMessage.Add("Password Changed Sucessfully!");

        //            var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

        //            return badResponse;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[HttpPost("SubscribeEmailLatter")]
        //public HttpResponseMessage SubscribeEmailLatter(ConsumerSubscribeViewModel subscribeViewModel)
        //{
        //    TransactionStatus transactionStatus = new TransactionStatus();

        //    try
        //    {
        //        var subscribe = BuiltSubscribeBo(subscribeViewModel);
        //        transactionStatus = _corporateService.SubscribeEmailLatter(subscribe);

               
        //        if (transactionStatus.Status == true)
        //        {
        //            transactionStatus.Status = true;
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            transactionStatus.ReturnMessage.Add("You are Successfully Subscribed.");
        //            var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
        //            return response;
        //        }
        //        else
        //        {
        //            transactionStatus.Status = false;
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            transactionStatus.ReturnMessage.Add("You are allready Subscribed.");
        //            var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
        //            return response;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        //[HttpPost("UnSubscribeEmailLatter")]
        //public HttpResponseMessage UnSubscribeEmailLatter(ConsumerSubscribeViewModel subscribeViewModel)
        //{
        //    TransactionStatus transactionStatus = new TransactionStatus();

        //    try
        //    {
        //        var subscribe = BuiltSubscribeBo(subscribeViewModel);
        //        transactionStatus = _corporateService.UnSubscribeEmailLatter(subscribe);


        //        if (transactionStatus.Status == true)
        //        {
        //            transactionStatus.Status = true;
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            transactionStatus.ReturnMessage.Add("You are Successfully UnSubscribed.");
        //            var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
        //            return response;
        //        }
        //        else
        //        {
        //            transactionStatus.Status = false;
        //            transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //            transactionStatus.ReturnMessage.Add("You are allready Subscribed.");
        //            var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
        //            return response;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}
        //[HttpPost("SearchHotels")]
        //public HttpResponseMessage HotelsLists(PropertyViewModel propViewModel)
        //{
        //    try
        //    {
        //        var consBo = BuiltPropBo(propViewModel);

        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.PropertyList(consBo));
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        //[HttpPost("HotelListing_Sort")]
        //public HttpResponseMessage HotelListing_Sort(PropertyViewModel propViewModel)
        //{
        //    var results = new SearchHotelsValidation().Validate(propViewModel);

        //    if (!results.IsValid)
        //    {
        //        propViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        propViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        propViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, propViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var consBo = BuiltPropBo(propViewModel);

        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.PropertyList_Sort(consBo));
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        //[HttpPost("PropertyDetails")]
        //public HttpResponseMessage HotelsDetails(PropertyDetailsViewModel propViewModel)
        //{
        //    try
        //    {
        //        var consBo = BuiltPropDetailsBo(propViewModel);
        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.PropertyComplete_Details(consBo));
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[HttpPost("BookingDetails")]
        //public HttpResponseMessage BookingDetails(BookNowDetailsViewModel booknowViewModel)
        //{
        //    try
        //    {
        //        var booknowBo = BuiltBookNowDetailsBo(booknowViewModel);
        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.BookingHotel_Details(booknowBo));
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[HttpPost("RoomesLists")]
        //public HttpResponseMessage RoomLists(PropertyDetailsViewModel1 propViewModel)
        //{
        //    try
        //    {
        //        var consBo = BuiltPropDetailsRoomBo(propViewModel);

        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.RoomList(consBo));
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[HttpPost("WebPreBooking")]
        //public HttpResponseMessage WebPreBooking(PrebookingViewModel preViewModel)
        //{
        //    var results = new PrebookingValidation().Validate(preViewModel);

        //    if (!results.IsValid)
        //    {
        //        preViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        preViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        preViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, preViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var consBo = BuiltPreBo(preViewModel);

        //        var jsonResultdata = JsonConvert.SerializeObject(_corporateService.PreBooking(consBo));
        //        var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
        //        responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
        //        return responsedata;
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[HttpGet("GetBookingInvoice")]
        //public HttpResponseMessage GetBookingInvoice(string Invce_Num, int Cons_Id)
        //{

        //    try
        //    {
        //        var jsonResult = JsonConvert.SerializeObject(_corporateService.GetBookingInvoice(Invce_Num, Cons_Id));
        //        if (jsonResult != null)
        //        {
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //            return response;
        //        }
        //        return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }

        //}

        //[HttpGet("GetAllFacility")]
        //public HttpResponseMessage GetAllFacility()
        //{
        //    try
        //    {
        //        var jsonResult = JsonConvert.SerializeObject(_corporateService.GetActiveFacilities());
        //        if (jsonResult != null)
        //        {
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //            return response;
        //        }
        //        return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        //[HttpGet("GetLocationByCity")]
        //public HttpResponseMessage GetLocationByCity(string name)
        //{
        //    try
        //    {
        //        var jsonResult = JsonConvert.SerializeObject(_corporateService.GetLocationByCity(name));
        //        if (jsonResult != null)
        //        {
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //            return response;
        //        }
        //        return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        //[HttpGet("GetHiddenGems")]
        //public HttpResponseMessage GetHiddenGems()
        //{
        //    try
        //    {
        //        var jsonResult = JsonConvert.SerializeObject(_corporateService.GetHiddenGems());
        //        if (jsonResult != null)
        //        {
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //            return response;
        //        }
        //        return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}
        //[HttpGet("GetRecommendedHotels")]
        //public HttpResponseMessage GetRecommendedHotels()
        //{
        //    try
        //    {
        //        var jsonResult = JsonConvert.SerializeObject(_corporateService.GetRecommendedHotels());
        //        if (jsonResult != null)
        //        {
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //            return response;
        //        }
        //        return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}
        //[HttpGet("GetBestOffers")]
        //public HttpResponseMessage GetBestOffers()
        //{
        //    try
        //    {
        //        var jsonResult = JsonConvert.SerializeObject(_corporateService.GetBestOffers());
        //        if (jsonResult != null)
        //        {
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //            return response;
        //        }
        //        return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        //[HttpGet("GetRoomPolicy")]
        //public HttpResponseMessage GetRoomPolicyById(int Prop_Id, int Room_Id)
        //{
        //    try
        //    {
        //        var jsonResult = JsonConvert.SerializeObject(_corporateService.GetRoomPolicyById(Prop_Id, Room_Id));
        //        if (jsonResult != null)
        //        {
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //            return response;
        //        }
        //        return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}
        //[HttpGet("GetRoomDetailsByID")]
        //public HttpResponseMessage GetRoomDetailsByID(int Prop_Id, int Room_Id)
        //{
        //    try
        //    {
        //        var jsonResult = JsonConvert.SerializeObject(_corporateService.GetRoomDetailsByID(Prop_Id, Room_Id));
        //        if (jsonResult != null)
        //        {
        //            var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
        //            return response;
        //        }
        //        return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        //[HttpGet("GetAutoCompleteSearch")]
        //public HttpResponseMessage GetAutoCompleteSearch(string terms)
        //{

        //    var autocompleteText = _corporateService.GetAutoCompleteLocation(terms);
        //    var badResponse = Request.CreateResponse(HttpStatusCode.OK, autocompleteText);

        //    return badResponse;
        //}
        //[HttpGet("GetAutoCompleteSearchResult")]
        //public HttpResponseMessage GetAutoCompleteLocationSearch(string terms)
        //{

        //    var autocompleteText = _corporateService.GetAutoCompleteLocationSearch(terms);
        //    var badResponse = Request.CreateResponse(HttpStatusCode.OK, autocompleteText);

        //    return badResponse;
        //}
        ////API
        //[HttpPost("AddContactUs")]
        //public HttpResponseMessage AddFeedBack(FeedBackViewModel feedbackViewModel)
        //{
        //    TransactionStatus transactionStatus = new TransactionStatus();
        //    var results = new FeedBackValidation().Validate(feedbackViewModel);
        //    if (!results.IsValid)
        //    {
        //        feedbackViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        feedbackViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        feedbackViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, feedbackViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var feedbackBo = BuiltFeedBackBo(feedbackViewModel);
        //        transactionStatus = _corporateService.AddFeedBack(feedbackBo);

        //        //if (transactionStatus.Status == false)
        //        //{
        //        //    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        //    transactionStatus.ReturnMessage.Add("Already Subscribed With This Email ");
        //        //    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, transactionStatus);
        //        //    return badResponse;
        //        //}
        //        try
        //        {
        //            var html = "<div style='width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:'segoe UI';'><div style='background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;'><a href='#' style='display:inline-block;'><img src='http://www.lastminutekeys.com/img/Mailer/logo.png' title='Last Minute Keys' style='display:block;' /></a> </div><p style='color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);'>Feedback recived from LMK Website <br/><br> Name:-" + feedbackBo.Name + "<br/><br/>Email:-" + feedbackBo.EmailAddress + "<br /> <br /><b>The Lastminutekeys Team</b><br/> www.lastminutekeys.com</p><div style='background:#192b3e;padding:10px;'><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;'>Follow Us</h4><ul style='padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;'><li style='float:left;width:33.3%;text-align:left;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;'></i> Follow us on Twitter</a></li><li style='float:left;width:33.3%;text-align:center;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Facebook</a></li><li style='float:left;width:33.3%;text-align:right;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Instagram</a></li><div style='clear:both;'></div></ul><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;'>Contact Our Customer Care</h4><p style='color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;'><a href='www.lastminutekeys.com/Home/Contact_Us' style='color:#FFF;font-size:14px;font-weight:400;'>Click here</a></p></div></div>";
        //            bool IsSent = SendEmails("Contact request From LMK Website", html, "lastminutekeys@katprotech.com");
        //          //  bool IsSent = SendEmails("Contact request From LMK Website", "Name:-" + feedbackBo.Name + "<br/><br/>Email:-" + feedbackBo.EmailAddress + "<br/><br/> Message:-" + feedbackBo.Message + "", "lastminutekeys@katprotech.com");
        //        }
        //        catch
        //        {

        //        }
        //        transactionStatus.Status = true;
        //        transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //        transactionStatus.ReturnMessage.Add("Feedback Added Successfully.");
        //        var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        ////FeedBack
        //[HttpPost("AddFeedBack")]
        //public HttpResponseMessage AddFeedBack_Feed(FeedBackViewModel feedbackViewModel)
        //{
        //    TransactionStatus transactionStatus = new TransactionStatus();
        //    var results = new AddFeedBackValidation().Validate(feedbackViewModel);
        //    if (!results.IsValid)
        //    {
        //        feedbackViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
        //        feedbackViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        feedbackViewModel.Status = false;
        //        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, feedbackViewModel);
        //        return badResponse;
        //    }
        //    try
        //    {
        //        var feedbackBo = BuiltFeedBackBo(feedbackViewModel);
        //        transactionStatus = _corporateService.AddFeedBack_Feed(feedbackBo);

        //        //if (transactionStatus.Status == false)
        //        //{
        //        //    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
        //        //    transactionStatus.ReturnMessage.Add("Already Subscribed With This Email ");
        //        //    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, transactionStatus);
        //        //    return badResponse;
        //        //}
        //        try
        //        {
        //            var html = "<div style='width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:'segoe UI';'><div style='background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;'><a href='#' style='display:inline-block;'><img src='http://www.lastminutekeys.com/img/Mailer/logo.png' title='Last Minute Keys' style='display:block;' /></a> </div><p style='color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);'>Feedback received from LMK Website <br/><br>Name:-" + feedbackBo.Name + "<br/><br/>Email:-" + feedbackBo.EmailAddress + "<br/><br/>Message:-" + feedbackBo.Message + "<br /> <br /><b>The Lastminutekeys Team</b><br/> www.lastminutekeys.com</p><div style='background:#192b3e;padding:10px;'><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;'>Follow Us</h4><ul style='padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;'><li style='float:left;width:33.3%;text-align:left;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;'></i> Follow us on Twitter</a></li><li style='float:left;width:33.3%;text-align:center;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Facebook</a></li><li style='float:left;width:33.3%;text-align:right;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Instagram</a></li><div style='clear:both;'></div></ul><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;'>Contact Our Customer Care</h4><p style='color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;'><a href='www.lastminutekeys.com/Home/Contact_Us' style='color:#FFF;font-size:14px;font-weight:400;'>Click here</a></p></div></div>";
        //            bool IsSent = SendEmails("Feedback From LMK Website", html, "lastminutekeys@katprotech.com");
        //        }
        //        catch
        //        {

        //        }
        //        transactionStatus.Status = true;
        //        transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
        //        transactionStatus.ReturnMessage.Add("Feedback Added Successfully.");
        //        var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationErrorLogServices.AppException(ex);
        //        return null;
        //    }
        //}

        [HttpGet("AllCompanyUser")]
        public HttpResponseMessage GetAllCorporateUser(string EmailId)
        {
            var list = _corporateService.GetAllCorporateUser(EmailId);
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }

        [HttpGet("GetAllCorporateCompanies")]
        public HttpResponseMessage GetAllCorporateCompanies()
        {
            var list = _corporateService.GetAllCorporateCompanies();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }

        [HttpGet("GetAllCorporateUserByCompany")]
        public HttpResponseMessage GetAllCorporateUserByCompany(string CompanyName)
        {
          
            var list = _corporateService.GetAllCorporateUserByCompany(CompanyName);
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }

         [HttpGet("UpdateCorporateUserToAdmin")]
        public HttpResponseMessage UpdateCorporateUserToAdmin(string CorpEmail, string CorpCompany)
        {

            var list = _corporateService.UpdateCorporateUserToAdmin(CorpEmail, CorpCompany);
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }

        

        [HttpGet("AllStates")]
        public HttpResponseMessage GetAllStates()
        {
            var list = _corporateService.GetStates();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }


        [HttpGet("AllPincodes")]
        public HttpResponseMessage GetAllPincodes()
        {
            var list = _corporateService.GetPincodes();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }

        //private ConsumerChangePasswordBo BuiltConsumerChangePasswordBo(ConsumerChangePasswordViewModel consViewModel)
        //{
        //    return (ConsumerChangePasswordBo)new ConsumerChangePasswordBo().InjectFrom(consViewModel);
        //}

        //private ConsumerSubscribeBo BuiltSubscribeBo(ConsumerSubscribeViewModel consViewModel)
        //{
        //    return (ConsumerSubscribeBo)new ConsumerSubscribeBo().InjectFrom(consViewModel);
        //}

        //private ConsumerFormBo BuiltConsumerFormBo(ConsumerFormViewModel cityViewModel)
        //{
        //    return (ConsumerFormBo)new ConsumerFormBo().InjectFrom(cityViewModel);
        //}

        //private BookNowDetailsBo BuiltBookNowDetailsBo(BookNowDetailsViewModel booknowVM)
        //{
        //    return (BookNowDetailsBo)new BookNowDetailsBo().InjectFrom(booknowVM);
        //}

        //private FeedBackBo BuiltFeedBackBo(FeedBackViewModel feedbackViewModel)
        //{
        //    return (FeedBackBo)new FeedBackBo().InjectFrom(feedbackViewModel);
        //}

        //private ConsumerLoginBo BuiltConsumerWebLoginBo(ConsumerWebLoginViewModel logintViewModel)
        //{
        //    return (ConsumerLoginBo)new ConsumerLoginBo().InjectFrom(logintViewModel);
        //}
        //#endregion

        private CorporateBo BuiltCorporateBo(CorporateLoginViewModel crporateViewModel)
        {
            return (CorporateBo)new CorporateBo().InjectFrom(crporateViewModel);
        }
        //private ConsumerMandetBo BuiltConsumerMandetBo(ConsumerMandetViewModel mandetViewModel)
        //{
        //    return (ConsumerMandetBo)new ConsumerMandetBo().InjectFrom(mandetViewModel);
        //}
        private CorporateLoginBo BuiltCorporateLoginBo(CorporateLoginViewModel logintViewModel)
        {
            return (CorporateLoginBo)new CorporateLoginBo().InjectFrom(logintViewModel);
        }
        //private ListingBo BuiltPropBo(PropertyViewModel propViewModel)
        //{
        //    return (ListingBo)new ListingBo().InjectFrom(propViewModel);
        //}
        //private PrebookingBo BuiltPreBo(PrebookingViewModel preViewModel)
        //{
        //    return (PrebookingBo)new PrebookingBo().InjectFrom(preViewModel);
        //}
        //private ListingDetailsRoomBo BuiltPropDetailsRoomBo(PropertyDetailsViewModel1 propViewModel)
        //{
        //    return (ListingDetailsRoomBo)new ListingDetailsRoomBo().InjectFrom(propViewModel);
        //}
        //private ListingDetailsBo BuiltPropDetailsBo(PropertyDetailsViewModel propViewModel)
        //{
        //    return (ListingDetailsBo)new ListingDetailsBo().InjectFrom(propViewModel);
        //}
        //private ConsumerDetailsBo BuiltConsumerDetailsBo(ConsumerDetailsViewModel consViewModel)
        //{
        //    return (ConsumerDetailsBo)new ConsumerDetailsBo().InjectFrom(consViewModel);
        //}

        //private ConsumerForgotpwdBo BuiltConsumerForgotpwdBo(ConsumerForgotpwdViewModel consViewModel)
        //{
        //    return (ConsumerForgotpwdBo)new ConsumerForgotpwdBo().InjectFrom(consViewModel);
        //}
    }
}