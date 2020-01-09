using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using App.BusinessObject;
using App.Common;
using App.UIServices;
using App.Web.ModelValidation;
using App.Web.ViewModels;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using System.Data;
using System.Text;
using App.Domain;
using App.UIServices.InterfaceServices;
using System.Net.Mail;
using CCA.Util;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;



namespace App.Web.Controllers
{

    [RoutePrefix("api/Consumer")]
    public class ConsumerApiController : ApiController
    {
        CCACrypto ccaCrypto = new CCACrypto();
        string workingKey = "817C1DFAD9D36A924621C42B79CD4C0E";//put in the 32bit alpha numeric key in the quotes provided here 	
        string ccaRequest = "";
        public string strEncRequest = "";
        public string strAccessCode = "AVMF04CA21BU64FMUB";// put the access key in the quotes provided here.
        readonly IConsumerService _consumerService;
        readonly ILoginServices _loginService;

        public ConsumerApiController(IConsumerService consumerervice, ILoginServices loginservice)
        {
            _consumerService = consumerervice;
            _loginService = loginservice;
        }
        //Create Consumer with all fields

        public ConsumerApiController()
        {

        }

        [HttpPost("createForAllFields")]
        public HttpResponseMessage CreateConsumer(ConsumerViewModel consumerViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new ConsumerValidation().Validate(consumerViewModel);

            if (!results.IsValid)
            {
                consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                consumerViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
                return badResponse;
            }
            try
            {
                var consBo = BuiltConsumerBo(consumerViewModel);
                transactionStatus = _consumerService.AddConsumer(consBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(consumerViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully inserted to database");

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
        [HttpPost("createMandetory")]
        public HttpResponseMessage CreateConsumerMandet(ConsumerMandetViewModel consumerViewModel)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            var results = new ConsumerMandetValidation().Validate(consumerViewModel);

            if (!results.IsValid)
            {
                consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                consumerViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
                return badResponse;
            }
            try
            {
                var consBo = BuiltConsumerMandetBo(consumerViewModel);

                DataSet dsUserDetails;
                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.AddConsumerMandet(consBo));
                //dsUserDetails = _consumerService.AddConsumerMandet(consBo);
                //if (dsUserDetails.Tables[0].Rows[0][0].ToString() != "10")
                //{
                //    bool IsSent = SendEmails("Mail From LMK", "Welcome to LMK <b>", consBo.Cons_mailid.ToString());
                //}



                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");

                return response;

            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }


        [HttpPost("consumerLogin")]
        public HttpResponseMessage ConsumerLogin(ConsumerLoginViewModel consumerloginViewModel)
        {
            List<Object> objUserId = new List<object>();


            var results = new ConsumerLoginValidation().Validate(consumerloginViewModel);

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
                var consBo = BuiltConsumerLoginBo(consumerloginViewModel);




                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.ConsumerLogin(consBo));
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

        [HttpPost("consumerDetails")]
        public HttpResponseMessage ConsumerDetails(ConsumerDetailsViewModel consumerDetailsViewModel)
        {
            List<Object> objUserId = new List<object>();

            DataSet dsUserDetails;
            var results = new ConsumerDetailsValidation().Validate(consumerDetailsViewModel);

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
                var consBo = BuiltConsumerDetailsBo(consumerDetailsViewModel);
                dsUserDetails = _consumerService.ConsumerDetails(consBo);

                objUserId.Add(dsUserDetails.Tables[0].Rows[0][0].ToString());

                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.ConsumerDetails(consBo));
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




        [HttpPost("consumerForgotPassword")]
        public HttpResponseMessage ConsumerForgotPassword(ConsumerForgotpwdViewModel consumerforgotpwdViewModel)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            List<Object> objUserpwd = new List<object>();

            DataSet dsUserDetails;
            var results = new ConsumerForgotpwdValidation().Validate(consumerforgotpwdViewModel);

            if (!results.IsValid)
            {
                objUserpwd.Add("required");
                var jsonResult = JsonConvert.SerializeObject(objUserpwd);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            try
            {
                var consBo = BuiltConsumerForgotpwdBo(consumerforgotpwdViewModel);
                dsUserDetails = _consumerService.ConsumerForgotpwd(consBo);

                objUserpwd.Add(dsUserDetails.Tables[0].Rows[0][0].ToString());
                if (dsUserDetails.Tables[0].Rows[0][0].ToString() != "not exists")
                {
                    var html = "<div style='width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:'segoe UI';'><div style='background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;'><a href='#' style='display:inline-block;'><img src='http://www.lastminutekeys.com/img/Mailer/logo.png' title='Last Minute Keys' style='display:block;' /></a> </div><p style='color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);'> Your  Lastminutekeys.com account password is : <b>" + dsUserDetails.Tables[0].Rows[0][0].ToString() + "</b> <br /> <br /><b>The Lastminutekeys Team</b><br/> www.lastminutekeys.com</p><div style='background:#192b3e;padding:10px;'><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;'>Follow Us</h4><ul style='padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;'><li style='float:left;width:33.3%;text-align:left;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;'></i> Follow us on Twitter</a></li><li style='float:left;width:33.3%;text-align:center;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Facebook</a></li><li style='float:left;width:33.3%;text-align:right;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Instagram</a></li><div style='clear:both;'></div></ul><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;'>Contact Our Customer Care</h4><p style='color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;'><a href='www.lastminutekeys.com/Home/Contact_Us' style='color:#FFF;font-size:14px;font-weight:400;'>Click here</a></p></div></div>";
                    bool IsSent = SendEmails("Mail From LMK", html, consBo.Cons_mailid.ToString());
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    if (IsSent)
                        transactionStatus.ReturnMessage.Add("Email Sent Successfully. Please Check Email");
                    else
                        transactionStatus.ReturnMessage.Add("Email Not Sent. Please Retry.");
                    //Admin
                    IsSent = SendEmails("Mail From LMK", html, "info@lastminutekeys.com");
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    //if (IsSent)
                    //    transactionStatus.ReturnMessage.Add("Email Sent Successfully. Please Check Email");
                    //else
                    //    transactionStatus.ReturnMessage.Add("Email Not Sent. Please Retry.");

                }
                else
                {
                    transactionStatus.ReturnMessage.Add("Mail doesn't Exist");
                }
                var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                return badResponse;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }
        //Mail method
        public bool SendEmails(string subject, string body, string reciver)
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
        //for Update Consumer

        [HttpPost("UpdateConsumer")]
        public HttpResponseMessage UpdateConsumer(ConsumerViewModel consumerViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new ConsumerValidation().Validate(consumerViewModel);

            if (!results.IsValid)
            {
                consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                consumerViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
                return badResponse;
            }
            try
            {
                var consBo = BuiltConsumerBo(consumerViewModel);
                transactionStatus = _consumerService.UpdateConsumer(consBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(consumerViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Profile successfully updated!!!");

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
        //for reset paswd
        [HttpPost("UpdateConsumerPswd")]
        public HttpResponseMessage UpdateConsumerPswd(ConsumerViewModel consumerViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new ConsumerChagePasswordValidation1().Validate(consumerViewModel);

            if (!results.IsValid)
            {
                consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                consumerViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
                return badResponse;
            }
            try
            {
                var consBo = BuiltConsumerBo(consumerViewModel);
                transactionStatus = _consumerService.UpdateConsumerPswd(consBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(consumerViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Password updated!!");

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

        [HttpPost("PropertyListing")]
        public HttpResponseMessage PropertyList(PropertyViewModel propViewModel)
        {

            List<object> objPropList = new List<object>();

            DataSet dsPropertyDetails;

            try
            {
                var consBo = BuiltPropBo(propViewModel);
                dsPropertyDetails = _consumerService.PropertyList(consBo);

                objPropList.Add(dsPropertyDetails);

                var jsonResultdata = JsonConvert.SerializeObject(objPropList);
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }


        //lISTING WITH SORT

        [HttpPost("PropertyListing_Sort")]
        public HttpResponseMessage PropertyList_Sort(PropertyViewModel propViewModel)
        {

            List<Object> objPropList = new List<object>();

            DataSet dsPropertyDetails;

            try
            {
                var consBo = BuiltPropBo(propViewModel);
                dsPropertyDetails = _consumerService.PropertyList_Sort(consBo);

                objPropList.Add(dsPropertyDetails);

                var jsonResultdata = JsonConvert.SerializeObject(objPropList);
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }
        [HttpPost("PropertyListingDetails")]
        public HttpResponseMessage PropertyListDetails(PropertyDetailsViewModel propViewModel)
        {

            List<Object> objPropList = new List<object>();

            DataSet dsPropertyDetails;


            try
            {
                var consBo = BuiltPropDetailsBo(propViewModel);
                dsPropertyDetails = _consumerService.PropertyListDetails(consBo);

                objPropList.Add(dsPropertyDetails);

                var jsonResultdata = JsonConvert.SerializeObject(objPropList);
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        //proc_Room_Listing
        [HttpPost("Room_Listing")]
        public HttpResponseMessage RoomList(PropertyDetailsViewModel1 propViewModel)
        {
            List<Object> objPropList = new List<object>();

            DataSet dsPropertyDetails;


            try
            {

                var consBo = BuiltPropDetailsRoomBo(propViewModel);
                dsPropertyDetails = _consumerService.RoomList(consBo);

                objPropList.Add(dsPropertyDetails);

                var jsonResultdata = JsonConvert.SerializeObject(objPropList);
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpGet("allcities")]
        public HttpResponseMessage GetCity()
        {
            var Citylist = _consumerService.GetCity();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, Citylist);
            return badResponse;

        }

        [HttpGet("alllocations")]
        public HttpResponseMessage GetLocations()
        {
            var Locatiolist = _consumerService.GetLocations();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, Locatiolist);
            return badResponse;
        }

        [HttpGet("SendInvoiceEmail")]
        public HttpResponseMessage SendInvoiceEmail(string InvoiceNumber, string Email)
        {
            var EmailTo = Email.Split(',');

            HttpResponseMessage response = null;
            string FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~\HTMLTemplate\ConfirmationInvoice.html");
            string EmailBody = File.ReadAllText(FilePath);
            DataSet objConsumerds = _consumerService.GetBookingInvoice(InvoiceNumber, 0);
            DataTable objConsumerdt = objConsumerds.Tables[0];

            DateTime checkInDate = Convert.ToDateTime(objConsumerdt.Rows[0]["Checkin"].ToString());
            DateTime checkOutDate = Convert.ToDateTime(objConsumerdt.Rows[0]["Checkout"].ToString());


            string checkIn = checkInDate.ToString("ddd, MMM dd yyyy");
            string checkOut = checkOutDate.ToString("ddd, MMM dd yyyy");



            EmailBody = EmailBody.Replace("$$ConsumerName$$", objConsumerdt.Rows[0]["Cons_First_Name"].ToString());
            EmailBody = EmailBody.Replace("$$HotelName$$", objConsumerdt.Rows[0]["Prop_Name"].ToString());
            EmailBody = EmailBody.Replace("$$Addres$$", objConsumerdt.Rows[0]["Prop_Addr1"].ToString());
            EmailBody = EmailBody.Replace("$$BookingId$$", objConsumerdt.Rows[0]["Invce_Num"].ToString());
            EmailBody = EmailBody.Replace("$$CheckinDate$$", checkIn);
            EmailBody = EmailBody.Replace("$$CheckOutDate$$", checkOut);
            EmailBody = EmailBody.Replace("$$NoOfRooms$$", objConsumerdt.Rows[0]["Room_Count"].ToString());
            EmailBody = EmailBody.Replace("$$NoOfNights$$", objConsumerdt.Rows[0]["Days_Count"].ToString());
            EmailBody = EmailBody.Replace("$$RoomType$$", objConsumerdt.Rows[0]["Room_Name"].ToString());
            EmailBody = EmailBody.Replace("$$NoOfPerson$$", objConsumerdt.Rows[0]["Room_Count"].ToString());

            ConsumerFormViewModel objConsumer = new ConsumerFormViewModel();


            objConsumer.Cons_Subject = "Booking Confirmation";
            decimal Total = Convert.ToDecimal(objConsumerdt.Rows[0]["camo_room_rate"].ToString()) * Convert.ToInt16(objConsumerdt.Rows[0]["Room_Count"].ToString()) * Convert.ToInt16(objConsumerdt.Rows[0]["Days_Count"].ToString());


            string Path = System.Web.Hosting.HostingEnvironment.MapPath(@"~\HTMLTemplate\ChargesSummary.html");
            string ChargesSummary = File.ReadAllText(Path);


            for (int i = 0; i < EmailTo.Length; i++)
            {
                var caseEmail = 0;
                int.TryParse(EmailTo[i].ToString(), out caseEmail);
                switch (caseEmail)
                {
                    case (int)AuthorityEnum.Admin:
                        objConsumer.Cons_mailid = "info@lastminutekeys.com";
                        ChargesSummary = ChargesSummary.Replace("$$RoomCost$$", objConsumerdt.Rows[0]["camo_room_rate"].ToString());
                        ChargesSummary = ChargesSummary.Replace("$$NoOfRooms$$", objConsumerdt.Rows[0]["Room_Count"].ToString());
                        ChargesSummary = ChargesSummary.Replace("$$NoOfNights$$", objConsumerdt.Rows[0]["Days_Count"].ToString());
                        ChargesSummary = ChargesSummary.Replace("$$Total$$", Total.ToString());
                        ChargesSummary = ChargesSummary.Replace("$$Taxes$$", objConsumerdt.Rows[0]["tax_amnt"].ToString());
                        ChargesSummary = ChargesSummary.Replace("$$PricePayable$$", objConsumerdt.Rows[0]["net_amt"].ToString());
                        EmailBody = EmailBody.Replace("$$ChargesSummary$$", ChargesSummary);
                        objConsumer.Cons_Body = EmailBody;
                        response = SendEmail(objConsumer);
                        break;
                    case (int)AuthorityEnum.Vendor:

                        string PathforVendor = System.Web.Hosting.HostingEnvironment.MapPath(@"~\HTMLTemplate\ConfirmationInvoice.html");
                        string EmailBodyVendor = File.ReadAllText(PathforVendor);
                        EmailBodyVendor = EmailBodyVendor.Replace("$$ConsumerName$$", objConsumerdt.Rows[0]["Cons_First_Name"].ToString());
                        EmailBodyVendor = EmailBodyVendor.Replace("$$HotelName$$", objConsumerdt.Rows[0]["Prop_Name"].ToString());
                        EmailBodyVendor = EmailBodyVendor.Replace("$$Addres$$", objConsumerdt.Rows[0]["Prop_Addr1"].ToString());
                        EmailBodyVendor = EmailBodyVendor.Replace("$$BookingId$$", objConsumerdt.Rows[0]["Invce_Num"].ToString());
                        EmailBodyVendor = EmailBodyVendor.Replace("$$CheckinDate$$", checkIn);
                        EmailBodyVendor = EmailBodyVendor.Replace("$$CheckOutDate$$", checkOut);
                        EmailBodyVendor = EmailBodyVendor.Replace("$$NoOfRooms$$", objConsumerdt.Rows[0]["Room_Count"].ToString());
                        EmailBodyVendor = EmailBodyVendor.Replace("$$NoOfNights$$", objConsumerdt.Rows[0]["Days_Count"].ToString());
                        EmailBodyVendor = EmailBodyVendor.Replace("$$RoomType$$", objConsumerdt.Rows[0]["Room_Name"].ToString());
                        EmailBodyVendor = EmailBodyVendor.Replace("$$NoOfPerson$$", objConsumerdt.Rows[0]["Room_Count"].ToString());
                        EmailBodyVendor = EmailBodyVendor.Replace("$$ChargesSummary$$", "<br/><br/>");
                        objConsumer.Cons_mailid = objConsumerdt.Rows[0]["prop_booking_mailid"].ToString();
                        objConsumer.Cons_Body = EmailBodyVendor;
                        response = SendEmail(objConsumer);
                        break;


                    case (int)AuthorityEnum.Consumer:

                        objConsumer.Cons_mailid = objConsumerdt.Rows[0]["Cons_mailid"].ToString();
                        ChargesSummary = ChargesSummary.Replace("$$RoomCost$$", objConsumerdt.Rows[0]["camo_room_rate"].ToString());
                        ChargesSummary = ChargesSummary.Replace("$$NoOfRooms$$", objConsumerdt.Rows[0]["Room_Count"].ToString());
                        ChargesSummary = ChargesSummary.Replace("$$NoOfNights$$", objConsumerdt.Rows[0]["Days_Count"].ToString());
                        ChargesSummary = ChargesSummary.Replace("$$Total$$", Total.ToString());
                        ChargesSummary = ChargesSummary.Replace("$$Taxes$$", objConsumerdt.Rows[0]["tax_amnt"].ToString());
                        ChargesSummary = ChargesSummary.Replace("$$PricePayable$$", objConsumerdt.Rows[0]["net_amt"].ToString());
                        EmailBody = EmailBody.Replace("$$ChargesSummary$$", ChargesSummary);
                        objConsumer.Cons_Body = EmailBody;
                        response = SendEmail(objConsumer);
                        break;

                }
            }

            return response;

        }

        [HttpGet("SendInvoiceSMS")]
        public HttpResponseMessage SendInvoiceSMS(string InvoiceNumber)
        {

            DataSet objConsumerds = _consumerService.GetBookingInvoice(InvoiceNumber, 0);
            DataTable objConsumerdt = objConsumerds.Tables[0];


            ConsumerFormViewModel objConsumer = new ConsumerFormViewModel();


            objConsumer.Cons_Subject = "Booking Confirmation";
            string CheckindDate = Convert.ToDateTime(objConsumerdt.Rows[0]["Checkin"].ToString()).ToString("dd-MM-yyyy");
            objConsumer.Cons_Body = "Thanks for booking with LMK. Your Booking is confirmed. Booking ID : " + objConsumerdt.Rows[0]["Invce_Num"].ToString() + " Check-In Date: " + CheckindDate + " for " + objConsumerdt.Rows[0]["Days_Count"].ToString() + " night at " + objConsumerdt.Rows[0]["Prop_Name"].ToString() + "";
            objConsumer.Cons_Mobile = objConsumerdt.Rows[0]["Cons_Mobile"].ToString();
            objConsumer.Cons_SMS_Gateway = objConsumerds.Tables[6].Rows[0]["Sms_Url"].ToString();
            return SendSMS(objConsumer);
        }

        [HttpPost("SendMail")]
        public HttpResponseMessage SendEmail(ConsumerFormViewModel cons)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
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

                var message = new MailMessage(senderID, cons.Cons_mailid);

                message.Subject = cons.Cons_Subject;
                message.Body = cons.Cons_Body;
                message.IsBodyHtml = true;

                smtp.Send(message);

                transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();

                transactionStatus.ReturnMessage.Add("Email Sent Successfully. Please Check Email");

                var goodresponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                return goodresponse;
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

        [HttpPost("SendSMS")]
        public HttpResponseMessage SendSMS(ConsumerFormViewModel cons)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            try
            {
                string username = "travelmartindia";
                string password = "LMK@167877";
                //string newsender = "hemant";
                //string domian = "smsgatewayurl.com";
                //var nmbr = "8123843095";
                //string baseurl = "http://login.smsgatewayhub.com/smsapi/pushsms.aspx?user=" + username + "&pwd=" + password + "&to=" + cons.Cons_Mobile + "&sid=LMKDMF&msg=" + cons.Cons_Body + "&fl=0&gwid=2";
                string baseurl = cons.Cons_SMS_Gateway + "&to=" + cons.Cons_Mobile + "&msg=" + cons.Cons_Body;
                WebClient client = new WebClient();
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                transactionStatus.ReturnMessage.Add("SMS Sent Successfully. Please Check Mobile");
                var goodresponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                return goodresponse;
            }
            catch (Exception ex)
            {
                var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                return badResponse;
            }

        }


        //Prebooking

        [HttpPost("PreBooking")]
        public HttpResponseMessage PreBooking(PrebookingViewModel preViewModel)
        {

            List<Object> BookingList = new List<object>();

            DataSet dsBookingDetails;

            try
            {
                var consBo = BuiltPreBo(preViewModel);
                dsBookingDetails = _consumerService.PreBooking(consBo);

                BookingList.Add(dsBookingDetails);

                var jsonResultdata = JsonConvert.SerializeObject(BookingList);
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }
        //Cc Avenue
        [HttpGet("CCAvenue")]
        public string CCAvenue(string Invce_Num, double Amount)
        {
            try
            {

                DataSet dsdata = new DataSet();
                dsdata = _consumerService.GetTransaction(Invce_Num);

                ccaRequest = ccaRequest + "merchant_id" + "=" + "56460&";


                ccaRequest = ccaRequest + "order_id" + "=" + Invce_Num + "&";
                double tot = Convert.ToDouble(dsdata.Tables[0].Rows[0]["net_amt"]);
                ccaRequest = ccaRequest + "amount" + "=" + tot + "&";
                ccaRequest = ccaRequest + "currency" + "=" + "INR&";
                ccaRequest = ccaRequest + "redirect_url" + "=" + "http://www.lastminutekeys.com/response&";
                ccaRequest = ccaRequest + "cancel_url" + "=" + "http://www.lastminutekeys.com/Cancel&";

                //Billing Details
                //  dsdata = (DataSet)Session["raiaddrs"];
                //ccaRequest = ccaRequest + "billing_name" + "=" + dsdata.Tables[0].Rows[0]["name"].ToString() + "&";
                //ccaRequest = ccaRequest + "billing_address" + "=" + dsdata.Tables[0].Rows[0]["address"].ToString() + "&";
                //ccaRequest = ccaRequest + "billing_city" + "=" + dsdata.Tables[0].Rows[0]["city"].ToString() + "&";
                //ccaRequest = ccaRequest + "billing_state" + "=" + dsdata.Tables[0].Rows[0]["state"].ToString() + "&";
                //ccaRequest = ccaRequest + "billing_zip" + "=" + dsdata.Tables[0].Rows[0]["pin"].ToString() + "&";
                //ccaRequest = ccaRequest + "billing_country" + "=" + "India&";
                //ccaRequest = ccaRequest + "billing_tel" + "=" + dsdata.Tables[0].Rows[0]["mob"].ToString() + "&";
                //ccaRequest = ccaRequest + "billing_email" + "=" + dsdata.Tables[0].Rows[0]["email"].ToString() + "&";

                ccaRequest = ccaRequest + "billing_name" + "=" + dsdata.Tables[0].Rows[0]["Cons_First_Name"].ToString() + "&";
                ccaRequest = ccaRequest + "billing_address" + "=" + dsdata.Tables[0].Rows[0]["Cons_Addr1"].ToString() + dsdata.Tables[0].Rows[0]["Cons_Addr2"].ToString() + "&";
                ccaRequest = ccaRequest + "billing_city" + "=" + dsdata.Tables[0].Rows[0]["Cons_City"].ToString() + "&";
                ccaRequest = ccaRequest + "billing_state" + "=" + "NULL&";
                ccaRequest = ccaRequest + "billing_zip" + "=" + "NULL&";
                ccaRequest = ccaRequest + "billing_country" + "=" + "India&";
                ccaRequest = ccaRequest + "billing_tel" + "=" + dsdata.Tables[0].Rows[0]["Cons_Mobile"].ToString() + "&";
                ccaRequest = ccaRequest + "billing_email" + "=" + dsdata.Tables[0].Rows[0]["Cons_mailid"].ToString() + "&";
                //Shipment Details

                //ccaRequest = ccaRequest + "delivery_name" + "=" + dsdata.Tables[0].Rows[0]["name"].ToString() + "&";
                //ccaRequest = ccaRequest + "delivery_address" + "=" + dsdata.Tables[0].Rows[0]["address"].ToString() + "&";
                //ccaRequest = ccaRequest + "delivery_city" + "=" + dsdata.Tables[0].Rows[0]["city"].ToString() + "&";
                //ccaRequest = ccaRequest + "delivery_state" + "=" + dsdata.Tables[0].Rows[0]["state"].ToString() + "&";
                //ccaRequest = ccaRequest + "delivery_zip" + "=" + dsdata.Tables[0].Rows[0]["pin"].ToString() + "&";
                //ccaRequest = ccaRequest + "delivery_country" + "=" + "India&";
                //ccaRequest = ccaRequest + "delivery_tel" + "=" + dsdata.Tables[0].Rows[0]["mob"].ToString() + "&";
                ccaRequest = ccaRequest + "delivery_name" + "=" + "=" + "NULL&";
                ccaRequest = ccaRequest + "delivery_address" + "=" + "NULL&";
                ccaRequest = ccaRequest + "delivery_city" + "=" + "NULL&";
                ccaRequest = ccaRequest + "delivery_state" + "=" + "NULL&";
                ccaRequest = ccaRequest + "delivery_zip" + "=" + "NULL&";
                ccaRequest = ccaRequest + "delivery_country" + "=" + "India&";
                ccaRequest = ccaRequest + "delivery_tel" + "=" + "NULL&&";

                ccaRequest = ccaRequest + "merchant_param1" + "=" + "additional Info.&";
                ccaRequest = ccaRequest + "merchant_param2" + "=" + "additional Info.&";
                ccaRequest = ccaRequest + "merchant_param3" + "=" + "additional Info.&";
                ccaRequest = ccaRequest + "merchant_param4" + "=" + "additional Info.&";
                ccaRequest = ccaRequest + "merchant_param5" + "=" + "additional Info.&";
                ccaRequest = ccaRequest + " promo_code" + "=" + "&";
                ccaRequest = ccaRequest + "customer_identifier" + "=" + dsdata.Tables[0].Rows[0]["Cons_mailid"].ToString() + "&";

                return strEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);
            }


            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }


        }

        [HttpGet("Acess_Code")]
        public string Access_Code()
        {
            return strAccessCode;
        }

        //public HttpResponseMessage CCAvenueRespone()
        //{
        //    string workingKey = "817C1DFAD9D36A924621C42B79CD4C0E";//put in the 32bit alpha numeric key in the quotes provided here
        //    CCACrypto ccaCrypto = new CCACrypto();
        //    string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
        //    Label1.Text = encResponse.ToString();
        //    //NameValueCollection Params = new NameValueCollection();
        //    string[] segments = encResponse.Split('&');
        //    string[] ordrid = segments[0].Split('=');
        //    string[] ordrstatus = segments[3].Split('=');
        //    Label1.Text = segments[0] + " <br/> " + segments[3] + " <br/> " + segments[4] + " <br/> " + encResponse.ToString();
        //    lblid.Value = ordrid[1];
        //    lblstatus.Value = ordrstatus[1];


        //}

        [HttpPost("PreBookingUpdate")]
        public HttpResponseMessage PreBookingUpdate(PrebookingViewModel preViewModel)
        {

            List<Object> BookingList = new List<object>();

            DataSet dsBookingDetails;

            try
            {
                var consBo = BuiltPreBo(preViewModel);
                dsBookingDetails = _consumerService.PreBookingUpdate(consBo);

                BookingList.Add(dsBookingDetails);

                var jsonResultdata = JsonConvert.SerializeObject(BookingList);
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpGet("GetTransaction")]
        public HttpResponseMessage GetTransaction(string Invce_Num)
        {

            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.GetTransaction(Invce_Num));
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
        [HttpPost("GetAllTransaction")]
        public HttpResponseMessage GetAllTransaction(PrebookingViewModel preViewModel)
        {

            List<Object> BookingList = new List<object>();

            DataSet dsBookingDetails;

            try
            {
                var consBo = BuiltPreBo(preViewModel);
                dsBookingDetails = _consumerService.GetAllTransaction(consBo);

                BookingList.Add(dsBookingDetails);

                var jsonResultdata = JsonConvert.SerializeObject(BookingList);
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        #region WebApi

        [HttpPost("SignUpConsumer")]
        public HttpResponseMessage SignUpConsumer(ConsumerMandetViewModel consumerViewModel)
        {

            var results = new ConsumerSignUpValidation().Validate(consumerViewModel);

            if (!results.IsValid)
            {
                consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                consumerViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
                return badResponse;
            }
            try
            {
                var consBo = BuiltConsumerMandetBo(consumerViewModel);



                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.AddConsumerMandet(consBo));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);

                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;

            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("WebLogin")]
        public HttpResponseMessage WebConsumerLogin(ConsumerWebLoginViewModel loginViewModel)
        {
            var results = new ConsumerWebLoginValidation().Validate(loginViewModel);

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

                ConsumerLoginViewModel viewModel = new ConsumerLoginViewModel();
                ConsumerController ctrl = new ConsumerController(_consumerService);
                viewModel.Cons_mailid = loginViewModel.Cons_mailid;
                viewModel.Cons_Pswd = loginViewModel.Cons_Pswd;
                string IsValiddate = ctrl.WebLoginProvider(viewModel, null);
                if (IsValiddate != "0")
                {
                    if (!string.IsNullOrEmpty(loginViewModel.returnUrl))
                    {
                        if (tempUrl.Contains("http://"))
                            loginViewModel.returnUrl = "Home";
                        if (tempUrl.Contains("/Signin"))
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

        [HttpGet("AuthenticateUser")]
        public HttpResponseMessage ExternalLogin(string UserId, string Name, string EmailId, string returnUrl)
        {
            string[] str = null; string tempUrl = returnUrl;
            ConsumerMandetBo consBo = new ConsumerMandetBo();
            string firstName = null, lastName = string.Empty;
            string[] strArray = Name.Split(' ');
            if (strArray.Length > 0)
                firstName = strArray[0];
            for (int i = 0; i < strArray.Length; i++)
            {
                lastName = strArray[1];
            }
            consBo.Cons_First_Name = firstName;
            consBo.Cons_Last_Name = string.IsNullOrEmpty(lastName) ? Name : lastName;
            consBo.Cons_mailid = EmailId;
            consBo.Cons_Pswd = UserId;

            try
            {
                DataSet ds = _consumerService.AddConsumerMandet(consBo);
                int isTrue = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                if (isTrue != -1)
                {
                    ConsumerLoginViewModel viewModel = new ConsumerLoginViewModel();
                    ConsumerController ctrl = new ConsumerController(_consumerService);
                    viewModel.Cons_mailid = EmailId;
                    viewModel.Cons_Pswd = UserId;
                    ctrl.LoginProvider(viewModel, null);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //if (tempUrl.Contains("http://"))
                        //    returnUrl = "";
                        if (tempUrl.Contains("/home"))
                            returnUrl = "Home";
                        if (tempUrl.Contains("/home"))
                            returnUrl = "/Home";
                        if (tempUrl.Contains("/Signin"))
                            returnUrl = "/my_account";
                        if (tempUrl.Contains("Booking_payment_method"))
                            returnUrl = "/Booking_payment_method";
                        if (tempUrl.Contains("/Search_Results"))
                            returnUrl = "Search_Results";
                        if (tempUrl.Contains("/hotel_details"))
                            returnUrl = "/hotel_details";

                    }
                    else
                        returnUrl = "my_account";
                    var jsonResultdata = JsonConvert.SerializeObject(returnUrl);
                    var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                    responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                    return responsedata;
                }
                return null;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("UpdateConsumerProfile")]
        public HttpResponseMessage UpdateConsumerProfile(ConsumerFormViewModel consumerViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new ConsumerUpdateProfileValidation().Validate(consumerViewModel);

            if (!results.IsValid)
            {
                consumerViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                consumerViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                consumerViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, consumerViewModel);
                return badResponse;
            }
            try
            {
                var consBo = BuiltConsumerFormBo(consumerViewModel);
                transactionStatus = _consumerService.UpdateConsumerProfile(consBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(consumerViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Profile successfully updated!!!");

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

        [HttpGet("GetBookedOverviewDeals")]
        public HttpResponseMessage GetBookedOverviewTransactionById(string Cons_Id)
        {
            try
            {
                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.GetOverviewBookedDealsById(Cons_Id));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpGet("GetBookedDeals")]
        public HttpResponseMessage GetBookedTransactionById(string Cons_Id)
        {
            try
            {
                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.GetBookedTransactionById(Cons_Id));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpGet("GetProfileDetails")]
        public HttpResponseMessage GetProfileDetails(string Cons_Id)
        {
            try
            {
                var ConsuBo = new ConsumerDetailsBo();
                //  ConsuBo.Cons_Id = Cons_Id;

                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.GetProfileDetails(Cons_Id));
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

        [HttpPost("ChangePassword")]
        public HttpResponseMessage ChangePassword(ConsumerChangePasswordViewModel changepasswordViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new ConsumerChagePasswordValidation().Validate(changepasswordViewModel);

            if (!results.IsValid)
            {
                changepasswordViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                changepasswordViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                changepasswordViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, changepasswordViewModel);
                return badResponse;
            }
            try
            {
                var consBo = BuiltConsumerChangePasswordBo(changepasswordViewModel);
                transactionStatus = _consumerService.ChangePassword(consBo);

                if (transactionStatus.Status == false)
                {
                    transactionStatus.Status = true;
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                    transactionStatus.ReturnMessage.Add("Current Password Is Wrong! Please Retry");

                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, transactionStatus);
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Password Changed Sucessfully!");

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

        [HttpPost("CheckSubscribeEmailLatter")]
        public HttpResponseMessage CheckSubscribeEmailLatter(ConsumerSubscribeViewModel subscribeViewModel)
        {
            TransactionStatus transactionStatus = new TransactionStatus();

            try
            {
                var subscribe = BuiltSubscribeBo(subscribeViewModel);
                transactionStatus = _consumerService.CheckSubscribeEmailLatter(subscribe);

                if (transactionStatus.Status == true)
                {
                    transactionStatus.Status = true;
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("1");
                    var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return response;
                }
                else
                {
                    transactionStatus.Status = false;
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("0");
                    var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return response;
                }

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("SubscribeEmailLatter")]
        public HttpResponseMessage SubscribeEmailLatter(ConsumerSubscribeViewModel subscribeViewModel)
        {
            TransactionStatus transactionStatus = new TransactionStatus();

            try
            {
                var subscribe = BuiltSubscribeBo(subscribeViewModel);
                transactionStatus = _consumerService.SubscribeEmailLatter(subscribe);


                if (transactionStatus.Status == true)
                {
                    transactionStatus.Status = true;
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("You are Successfully Subscribed.");
                    var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return response;
                }
                else
                {
                    transactionStatus.Status = false;
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("You are allready Subscribed.");
                    var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return response;
                }

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("UnSubscribeEmailLatter")]
        public HttpResponseMessage UnSubscribeEmailLatter(ConsumerSubscribeViewModel subscribeViewModel)
        {
            TransactionStatus transactionStatus = new TransactionStatus();

            try
            {
                var subscribe = BuiltSubscribeBo(subscribeViewModel);
                transactionStatus = _consumerService.UnSubscribeEmailLatter(subscribe);


                if (transactionStatus.Status == true)
                {
                    transactionStatus.Status = true;
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("You are Successfully UnSubscribed.");
                    var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return response;
                }
                else
                {
                    transactionStatus.Status = false;
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("You are allready Subscribed.");
                    var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return response;
                }

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }
        [HttpPost("SearchHotels")]
        public HttpResponseMessage HotelsLists(PropertyViewModel propViewModel)
        {
            try
            {
                var consBo = BuiltPropBo(propViewModel);

                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.PropertyList(consBo));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("HotelListing_Sort")]
        public HttpResponseMessage HotelListing_Sort(PropertyViewModel propViewModel)
        {
            var results = new SearchHotelsValidation().Validate(propViewModel);

            if (!results.IsValid)
            {
                propViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                propViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                propViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, propViewModel);
                return badResponse;
            }
            try
            {
                var consBo = BuiltPropBo(propViewModel);

                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.PropertyList_Sort(consBo));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("PropertyDetails")]
        public HttpResponseMessage HotelsDetails(PropertyDetailsViewModel propViewModel)
        {
            try
            {
                var consBo = BuiltPropDetailsBo(propViewModel);
                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.PropertyComplete_Details(consBo));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpPost("BookingDetails")]
        public HttpResponseMessage BookingDetails(BookNowDetailsViewModel booknowViewModel)
        {
            try
            {
                var booknowBo = BuiltBookNowDetailsBo(booknowViewModel);
                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.BookingHotel_Details(booknowBo));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpPost("RoomesLists")]
        public HttpResponseMessage RoomLists(PropertyDetailsViewModel1 propViewModel)
        {
            try
            {
                var consBo = BuiltPropDetailsRoomBo(propViewModel);

                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.RoomList(consBo));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpPost("WebPreBooking")]
        public HttpResponseMessage WebPreBooking(PrebookingViewModel preViewModel)
        {
            var results = new PrebookingValidation().Validate(preViewModel);

            if (!results.IsValid)
            {
                preViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                preViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                preViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, preViewModel);
                return badResponse;
            }
            try
            {
                var consBo = BuiltPreBo(preViewModel);

                var jsonResultdata = JsonConvert.SerializeObject(_consumerService.PreBooking(consBo));
                var responsedata = this.Request.CreateResponse(HttpStatusCode.OK);
                responsedata.Content = new StringContent(jsonResultdata, Encoding.UTF8, "application/json");
                return responsedata;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpGet("GetBookingInvoice")]
        public HttpResponseMessage GetBookingInvoice(string Invce_Num, int Cons_Id)
        {

            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.GetBookingInvoice(Invce_Num, Cons_Id));
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

        [HttpGet("CheckBookingStatus")]
        public HttpResponseMessage CheckBookingStatus(string Invce_Num, int Cons_Id)
        {

            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.CheckBookingStatus(Invce_Num, Cons_Id));
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

        [HttpGet("CheckCorporateUser")]
        public HttpResponseMessage CheckCorporateUser(string Cons_Id)
        {

            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.CheckCorporateUser(Cons_Id));
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

        [HttpGet("GetAllFacility")]
        public HttpResponseMessage GetAllFacility()
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.GetActiveFacilities());
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

        [HttpGet("GetLocationByCity")]
        public HttpResponseMessage GetLocationByCity(string name)
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.GetLocationByCity(name));
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

        [HttpGet("GetHiddenGems")]
        public HttpResponseMessage GetHiddenGems()
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.GetHiddenGems());
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
        [HttpGet("GetRecommendedHotels")]
        public HttpResponseMessage GetRecommendedHotels()
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.GetRecommendedHotels());
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
        [HttpGet("GetBestOffers")]
        public HttpResponseMessage GetBestOffers()
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.GetBestOffers());
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

        [HttpGet("GetRoomPolicy")]
        public HttpResponseMessage GetRoomPolicyById(int Prop_Id, int Room_Id)
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.GetRoomPolicyById(Prop_Id, Room_Id));
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
        [HttpGet("GetRoomDetailsByID")]
        public HttpResponseMessage GetRoomDetailsByID(int Prop_Id, int Room_Id)
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_consumerService.GetRoomDetailsByID(Prop_Id, Room_Id));
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

        [HttpGet("GetAutoCompleteSearch")]
        public HttpResponseMessage GetAutoCompleteSearch(string terms)
        {

            var autocompleteText = _consumerService.GetAutoCompleteLocation(terms);
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, autocompleteText);

            return badResponse;
        }
        [HttpGet("GetAutoCompleteSearchResult")]
        public HttpResponseMessage GetAutoCompleteLocationSearch(string terms)
        {

            var autocompleteText = _consumerService.GetAutoCompleteLocationSearch(terms);
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, autocompleteText);

            return badResponse;
        }
        //API
        [HttpPost("AddContactUs")]
        public HttpResponseMessage AddFeedBack(FeedBackViewModel feedbackViewModel)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            var results = new FeedBackValidation().Validate(feedbackViewModel);
            if (!results.IsValid)
            {
                feedbackViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                feedbackViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                feedbackViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, feedbackViewModel);
                return badResponse;
            }
            try
            {
                var feedbackBo = BuiltFeedBackBo(feedbackViewModel);
                transactionStatus = _consumerService.AddFeedBack(feedbackBo);

                //if (transactionStatus.Status == false)
                //{
                //    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                //    transactionStatus.ReturnMessage.Add("Already Subscribed With This Email ");
                //    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, transactionStatus);
                //    return badResponse;
                //}
                try
                {
                    var html = "<div style='width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:'segoe UI';'><div style='background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;'><a href='#' style='display:inline-block;'><img src='http://www.lastminutekeys.com/img/Mailer/logo.png' title='Last Minute Keys' style='display:block;' /></a> </div><p style='color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);'>Feedback recived from LMK Website <br/><br> Name:-" + feedbackBo.Name + "<br/><br/>Email:-" + feedbackBo.EmailAddress + "<br /> <br /><b>The Lastminutekeys Team</b><br/> www.lastminutekeys.com</p><div style='background:#192b3e;padding:10px;'><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;'>Follow Us</h4><ul style='padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;'><li style='float:left;width:33.3%;text-align:left;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;'></i> Follow us on Twitter</a></li><li style='float:left;width:33.3%;text-align:center;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Facebook</a></li><li style='float:left;width:33.3%;text-align:right;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Instagram</a></li><div style='clear:both;'></div></ul><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;'>Contact Our Customer Care</h4><p style='color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;'><a href='www.lastminutekeys.com/Home/Contact_Us' style='color:#FFF;font-size:14px;font-weight:400;'>Click here</a></p></div></div>";
                    bool IsSent = SendEmails("Contact request From LMK Website", html, "lastminutekeys@katprotech.com");
                    //  bool IsSent = SendEmails("Contact request From LMK Website", "Name:-" + feedbackBo.Name + "<br/><br/>Email:-" + feedbackBo.EmailAddress + "<br/><br/> Message:-" + feedbackBo.Message + "", "lastminutekeys@katprotech.com");
                }
                catch
                {

                }
                transactionStatus.Status = true;
                transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                transactionStatus.ReturnMessage.Add("Feedback Added Successfully.");
                var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                return response;

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //FeedBack
        [HttpPost("AddFeedBack")]
        public HttpResponseMessage AddFeedBack_Feed(FeedBackViewModel feedbackViewModel)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            var results = new AddFeedBackValidation().Validate(feedbackViewModel);
            if (!results.IsValid)
            {
                feedbackViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                feedbackViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                feedbackViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, feedbackViewModel);
                return badResponse;
            }
            try
            {
                var feedbackBo = BuiltFeedBackBo(feedbackViewModel);
                transactionStatus = _consumerService.AddFeedBack_Feed(feedbackBo);

                //if (transactionStatus.Status == false)
                //{
                //    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                //    transactionStatus.ReturnMessage.Add("Already Subscribed With This Email ");
                //    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, transactionStatus);
                //    return badResponse;
                //}
                try
                {
                    var html = "<div style='width:45%;margin:0 auto;font-size:14px;color:#222;line-height:1.6em;font-family:'segoe UI';'><div style='background:url(http://www.lastminutekeys.com/img/Mailer/header-banner.png) repeat-x;padding:12px;background-size: cover;background-position: 100% 100%;'><a href='#' style='display:inline-block;'><img src='http://www.lastminutekeys.com/img/Mailer/logo.png' title='Last Minute Keys' style='display:block;' /></a> </div><p style='color:#565656;margin:0px auto;padding:15px;font-size: 14px;line-height: 1.6em;box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-moz-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);-o-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);'>Feedback received from LMK Website <br/><br>Name:-" + feedbackBo.Name + "<br/><br/>Email:-" + feedbackBo.EmailAddress + "<br/><br/>Message:-" + feedbackBo.Message + "<br /> <br /><b>The Lastminutekeys Team</b><br/> www.lastminutekeys.com</p><div style='background:#192b3e;padding:10px;'><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;text-transform: uppercase;'>Follow Us</h4><ul style='padding:0;list-style:none;border-bottom:1px solid #FFF;border-top:1px solid #FFF;margin:5px 0;padding:5px 0;'><li style='float:left;width:33.3%;text-align:left;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -2px -6px;display:inline-block;vertical-align: sub;'></i> Follow us on Twitter</a></li><li style='float:left;width:33.3%;text-align:center;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -24px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Facebook</a></li><li style='float:left;width:33.3%;text-align:right;margin-left:0;'><a href='#' style='display:block;color:#F5F5F5;font-size:12px;text-decoration:none;'><i style='width:15px;height:15px;background:url(img/Mailer/social-icons.png) no-repeat -53px -5px;display:inline-block;vertical-align: sub;'></i> Follow us on Instagram</a></li><div style='clear:both;'></div></ul><h4 style='margin: 0;text-align: center;color: #FFFFFF;font-weight: 400;font-size: 15px;'>Contact Our Customer Care</h4><p style='color:#fefefe;text-align:center;font-size: 12px;margin:0;font-style:italic;padding:4px 0;'><a href='www.lastminutekeys.com/Home/Contact_Us' style='color:#FFF;font-size:14px;font-weight:400;'>Click here</a></p></div></div>";
                    bool IsSent = SendEmails("Feedback From LMK Website", html, "lastminutekeys@katprotech.com");
                }
                catch
                {

                }
                transactionStatus.Status = true;
                transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                transactionStatus.ReturnMessage.Add("Feedback Added Successfully.");
                var response = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                return response;

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpGet("AllStates")]
        public HttpResponseMessage GetAllStates()
        {
            var list = _consumerService.GetStates();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }
        [HttpGet("AllPincodes")]
        public HttpResponseMessage GetAllPincodes()
        {
            var list = _consumerService.GetPincodes();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }

        [HttpGet("GeneratePDF")]
        public String GeneratePDF(string Invce_Num, int Cons_Id)
        {
            DataSet ds = _consumerService.GetBookingInvoice(Invce_Num, Cons_Id);
            DataTable dt0 = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            DataTable dt2 = ds.Tables[2];
            DataTable dt3 = ds.Tables[3];
            DataTable dt4 = ds.Tables[4];

            var documentUrl = String.Empty;
            try
            {
                //Render PlaceHolder to temporary stream 
                using (var myMemoryStream = new MemoryStream())
                {
                    //Create PDF document 
                    var docmnt = new Document(PageSize.A4);

                    PdfWriter writer = PdfWriter.GetInstance(docmnt, myMemoryStream);
                    docmnt.Open();

                    //hospital information
                    // add a blank lines
                    docmnt.Add(Chunk.NEWLINE);
                    var invclogo = new PdfPTable(4);
                    var celllogo = new PdfPCell(new Phrase("",
                                        new Font(Font.FontFamily.TIMES_ROMAN, 20F, 0, WebColors.GetRGBColor("#FFFFFF"))));
                    celllogo.BackgroundColor = WebColors.GetRGBColor("#848484");
                    var img = "";
                    docmnt.Add(celllogo);

                    var invcbill = new PdfPTable(4);

                    var cell = new PdfPCell(new Phrase("INVOICE BILL",
                                        new Font(Font.FontFamily.TIMES_ROMAN, 20F, 0, WebColors.GetRGBColor("#FFFFFF"))));
                    cell.Colspan = 4;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = WebColors.GetRGBColor("#2781CD");

                    cell.Border = 0;
                    invcbill.DefaultCell.Border = Rectangle.NO_BORDER;
                    invcbill.AddCell(cell);
                    docmnt.Add(Chunk.NEWLINE);
                    docmnt.Add(invcbill);

                    var bookingdetails = new PdfPTable(4);
                    if (dt0.Rows.Count > 0)
                    {
                        var cell1 = new PdfPCell(new Phrase("Booking Details",
                                        new Font(Font.FontFamily.TIMES_ROMAN, 15F)));
                        cell1.Colspan = 4;
                        cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell1.BackgroundColor = WebColors.GetRGBColor("#848484");
                        cell1.Border = 0;
                        cell1.BorderColor = WebColors.GetRGBColor("#FFFFFF");
                        bookingdetails.DefaultCell.Border = Rectangle.NO_BORDER;

                        bookingdetails.AddCell(cell1);

                        DataRow drbookingdetails = dt0.Rows[0];
                        bookingdetails.AddCell(drbookingdetails["Prop_Name"].ToString());
                        bookingdetails.AddCell(String.Empty);


                        bookingdetails.AddCell(String.Empty);
                        bookingdetails.AddCell(drbookingdetails["Invce_Num"].ToString());

                        docmnt.Add(Chunk.NEWLINE);

                        bookingdetails.AddCell(drbookingdetails["Prop_Addr1"].ToString());
                        bookingdetails.AddCell(String.Empty);
                        bookingdetails.AddCell(String.Empty);
                        bookingdetails.AddCell(String.Empty);

                    }

                    try
                    {
                        if (dt3.Rows.Count > 0)
                        {
                            DataRow drimage = dt3.Rows[0];
                            Image logo = Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath(drimage["roomimage"].ToString()));
                            logo.ScaleAbsolute(100, 71);
                            bookingdetails.AddCell(logo);
                            bookingdetails.AddCell(String.Empty);
                            bookingdetails.AddCell(String.Empty);
                            bookingdetails.AddCell(String.Empty);
                        }
                    }
                    catch (Exception)
                    {
                        bookingdetails.AddCell(String.Empty);
                    }

                    if (dt0.Rows.Count > 0)
                    {
                        DataRow drbookingdetails = dt0.Rows[0];
                        string facilities = string.Empty;
                        bookingdetails.AddCell("Room Type:");
                        bookingdetails.AddCell(drbookingdetails["Room_Name"].ToString());

                        bookingdetails.AddCell("Facilities:");
                        foreach (DataRow dr1 in dt2.Rows)
                        {
                            facilities += dr1["Facility_Name"].ToString() + ',';
                        }

                        bookingdetails.AddCell(facilities.Substring(0, facilities.Length - 1));

                        bookingdetails.AddCell("Booking Id:");
                        bookingdetails.AddCell(drbookingdetails["Invce_Num"].ToString());
                        bookingdetails.AddCell(String.Empty); bookingdetails.AddCell(String.Empty);

                        bookingdetails.AddCell("Check In:");
                        bookingdetails.AddCell(drbookingdetails["Checkin"].ToString());
                        bookingdetails.AddCell(String.Empty); bookingdetails.AddCell(String.Empty);

                        bookingdetails.AddCell("Check Out:");
                        bookingdetails.AddCell(drbookingdetails["Checkout"].ToString());
                        bookingdetails.AddCell(String.Empty); bookingdetails.AddCell(String.Empty);

                        docmnt.Add(bookingdetails);
                        docmnt.Add(Chunk.NEWLINE);
                    }
                    docmnt.Add(Chunk.NEWLINE);

                    if (dt0.Rows.Count > 0)
                    {
                        var billinginfo = new PdfPTable(4);
                        var cell2 = new PdfPCell(new Phrase("Booking Info",
                                            new Font(Font.FontFamily.TIMES_ROMAN, 15F)));
                        cell2.Colspan = 4;
                        cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell2.BackgroundColor = WebColors.GetRGBColor("#848484");
                        cell2.Border = 0;
                        billinginfo.DefaultCell.Border = Rectangle.NO_BORDER;

                        billinginfo.AddCell(cell2);
                        docmnt.Add(Chunk.NEWLINE);
                        DataRow drbillinginfo = dt0.Rows[0];
                        billinginfo.AddCell("Name:");
                        billinginfo.AddCell(drbillinginfo["Cons_First_Name"].ToString());

                        billinginfo.AddCell("Occupant Name:");
                        billinginfo.AddCell(drbillinginfo["GuestName"].ToString());
                        //docmnt.Add(billinginfo);
                        docmnt.Add(Chunk.NEWLINE);

                        billinginfo.AddCell("Email:");
                        billinginfo.AddCell(drbillinginfo["Cons_mailid"].ToString());
                        billinginfo.AddCell(String.Empty); bookingdetails.AddCell(String.Empty);

                        billinginfo.AddCell("Mobile No:");
                        billinginfo.AddCell(drbillinginfo["Cons_Mobile"].ToString());
                        billinginfo.AddCell(String.Empty); bookingdetails.AddCell(String.Empty);
                        docmnt.Add(billinginfo);
                        docmnt.Add(Chunk.NEWLINE);
                    }
                    docmnt.Add(Chunk.NEWLINE);
                    if (dt0.Rows.Count > 0)
                    {
                        var pricedetails = new PdfPTable(4);
                        var cell3 = new PdfPCell(new Phrase("Price Details",
                                            new Font(Font.FontFamily.TIMES_ROMAN, 15F)));
                        cell3.Colspan = 4;
                        cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell3.BackgroundColor = WebColors.GetRGBColor("#848484");
                        cell3.Border = 0;
                        pricedetails.DefaultCell.Border = Rectangle.NO_BORDER;

                        pricedetails.AddCell(cell3);
                        docmnt.Add(Chunk.NEWLINE);
                        DataRow drpricedetails = dt0.Rows[0];
                        pricedetails.AddCell("Room Charges:");
                        pricedetails.AddCell(String.Empty);
                        pricedetails.AddCell(String.Empty);
                        pricedetails.AddCell(drpricedetails["prop_room_rate"].ToString());
                        docmnt.Add(Chunk.NEWLINE);

                        pricedetails.AddCell("Service Tax:");
                        pricedetails.AddCell(String.Empty);
                        pricedetails.AddCell(String.Empty);
                        pricedetails.AddCell(drpricedetails["tax_amnt"].ToString());
                        docmnt.Add(Chunk.NEWLINE);

                        pricedetails.AddCell("Grand Total:");
                        pricedetails.AddCell(String.Empty);
                        pricedetails.AddCell(String.Empty);
                        pricedetails.AddCell(drpricedetails["net_amt"].ToString());

                        docmnt.Add(pricedetails);
                        docmnt.Add(Chunk.NEWLINE);
                    }
                    docmnt.Close();
                    byte[] content = myMemoryStream.ToArray();
                    Stream stream = new MemoryStream(content);
                    var blobId = "bookingdetails" + ".pdf";
                    var contentType = "application/pdf";
                    var isUpload = BlobUtilities.CreateBlob("invoice", blobId, contentType, stream);
                    documentUrl = BlobUtilities.RetrieveBlobUrl("invoice", blobId);
                }
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
            return documentUrl;
        }

        private ConsumerChangePasswordBo BuiltConsumerChangePasswordBo(ConsumerChangePasswordViewModel consViewModel)
        {
            return (ConsumerChangePasswordBo)new ConsumerChangePasswordBo().InjectFrom(consViewModel);
        }

        private ConsumerSubscribeBo BuiltSubscribeBo(ConsumerSubscribeViewModel consViewModel)
        {
            return (ConsumerSubscribeBo)new ConsumerSubscribeBo().InjectFrom(consViewModel);
        }

        private ConsumerFormBo BuiltConsumerFormBo(ConsumerFormViewModel cityViewModel)
        {
            return (ConsumerFormBo)new ConsumerFormBo().InjectFrom(cityViewModel);
        }

        private BookNowDetailsBo BuiltBookNowDetailsBo(BookNowDetailsViewModel booknowVM)
        {
            return (BookNowDetailsBo)new BookNowDetailsBo().InjectFrom(booknowVM);
        }

        private FeedBackBo BuiltFeedBackBo(FeedBackViewModel feedbackViewModel)
        {
            return (FeedBackBo)new FeedBackBo().InjectFrom(feedbackViewModel);
        }

        private ConsumerLoginBo BuiltConsumerWebLoginBo(ConsumerWebLoginViewModel logintViewModel)
        {
            return (ConsumerLoginBo)new ConsumerLoginBo().InjectFrom(logintViewModel);
        }
        #endregion

        private ConsumerBo BuiltConsumerBo(ConsumerViewModel cityViewModel)
        {
            return (ConsumerBo)new ConsumerBo().InjectFrom(cityViewModel);
        }
        private ConsumerMandetBo BuiltConsumerMandetBo(ConsumerMandetViewModel mandetViewModel)
        {
            return (ConsumerMandetBo)new ConsumerMandetBo().InjectFrom(mandetViewModel);
        }
        private ConsumerLoginBo BuiltConsumerLoginBo(ConsumerLoginViewModel logintViewModel)
        {
            return (ConsumerLoginBo)new ConsumerLoginBo().InjectFrom(logintViewModel);
        }
        private ListingBo BuiltPropBo(PropertyViewModel propViewModel)
        {
            return (ListingBo)new ListingBo().InjectFrom(propViewModel);
        }
        private PrebookingBo BuiltPreBo(PrebookingViewModel preViewModel)
        {
            return (PrebookingBo)new PrebookingBo().InjectFrom(preViewModel);
        }
        private ListingDetailsRoomBo BuiltPropDetailsRoomBo(PropertyDetailsViewModel1 propViewModel)
        {
            return (ListingDetailsRoomBo)new ListingDetailsRoomBo().InjectFrom(propViewModel);
        }
        private ListingDetailsBo BuiltPropDetailsBo(PropertyDetailsViewModel propViewModel)
        {
            return (ListingDetailsBo)new ListingDetailsBo().InjectFrom(propViewModel);
        }
        private ConsumerDetailsBo BuiltConsumerDetailsBo(ConsumerDetailsViewModel consViewModel)
        {
            return (ConsumerDetailsBo)new ConsumerDetailsBo().InjectFrom(consViewModel);
        }

        private ConsumerForgotpwdBo BuiltConsumerForgotpwdBo(ConsumerForgotpwdViewModel consViewModel)
        {
            return (ConsumerForgotpwdBo)new ConsumerForgotpwdBo().InjectFrom(consViewModel);
        }



    }
}
