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

    [RoutePrefix("api/Booking")]
    public class BookingApiController : ApiController
    {

        readonly IBookingService _bookingService;

        readonly IVendorService _vendorService;



        public BookingApiController(IBookingService bkingService, IVendorService vendorService)
        {
            _bookingService = bkingService;
            _vendorService = vendorService;

        }


        [HttpPost("BookingList")]
        public HttpResponseMessage Bind(int VendID, string InvFrom, string InvTo)
        {
            try
            {
                InvFrom = InvFrom == "null" ? "" : InvFrom;
                InvTo = InvTo == "null" ? "" : InvTo;
                //Checkin = Checkin == "null" ? "" : Checkin;
                //Checkout = Checkout == "null" ? "" : Checkout;
                var jsonResult = JsonConvert.SerializeObject(_bookingService.Bind(VendID, InvFrom, InvTo));
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

        [HttpPost("CorpBookingList")]
        public HttpResponseMessage corpBookinglist(string Cons_Id,string bookingStatus, string InvFrom, string InvTo)
        {
            try
            {
                string compName = string.Empty;
                if (!string.IsNullOrEmpty(Cons_Id))
                {
                    var domain = Cons_Id.Split('@')[1];
                    compName = domain.Split('.')[0];
                }

                InvFrom = InvFrom == "null" ? "" : InvFrom;
                InvTo = InvTo == "null" ? "" : InvTo;
                var jsonResult = JsonConvert.SerializeObject(_bookingService.corpBookinglist(compName, bookingStatus,InvFrom, InvTo));
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

        [HttpGet("ConsumerReport")]
        public HttpResponseMessage ConsumerReport(string VendID, string cons_id, string cons_name, string cons_mailid, string cons_mobile, string days)
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_bookingService.ConsumerReport(VendID, cons_id, cons_name, cons_mailid, cons_mobile, days));
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
        [HttpGet("Tax_Report")]
        public HttpResponseMessage TaxReport(string VendID, string TaxType, string days)
        {

            try
            {
                var jsonResult = JsonConvert.SerializeObject(_bookingService.Tax_Report(VendID, TaxType, days));
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
        [HttpGet("CCAvenue_Report")]
        public HttpResponseMessage CCAvenue_Report(string VendID, string days)
        {

            try
            {
                var jsonResult = JsonConvert.SerializeObject(_bookingService.CCAvenue_Report(VendID, days));
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
        [HttpGet("CCAvenue_Report_Margin")]
        public HttpResponseMessage CCAvenue_Report_Margin(string VendID, string fromdate, string todate)
        {

            try
            {
                var jsonResult = JsonConvert.SerializeObject(_bookingService.LMK_Margin_Report(VendID, fromdate, todate));
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

        [HttpGet("UpcomingBookingList")]
        public HttpResponseMessage UpcomingBookingList(int VendID)
        {

            try
            {
                var jsonResult = JsonConvert.SerializeObject(_bookingService.UpcomingBookingList(VendID));
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
        [HttpGet("allproperty")]
        public HttpResponseMessage GetPropNames()
        {

            var list = _bookingService.BindProperty();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }
        [HttpGet("DashBoard")]
        public DataSet DashBoard(string vndr_Id, string Prop_Id, int Days)
        {

            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@vndr_Id",""),//0
                     new SqlParameter("@prop_Id",""),//0
                        new SqlParameter("@days",90),//0
                 //new SqlParameter("@term",Url),//0
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_dashboard]", Params);

            return ds;
        }
        [HttpGet("CheckMargin")]
        public DataSet UpdateRates(int Inv_Id)
        {
            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@inventory_id",Inv_Id),//0
                   
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_margin_superadmin]", Params);

            return ds;

        }

    }
}
