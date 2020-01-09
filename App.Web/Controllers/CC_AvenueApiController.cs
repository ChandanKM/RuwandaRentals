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
namespace App.Web.Controllers
{
    [RoutePrefix("api/CCAvenue")]
    public class CC_AvenueApiController : ApiController
    {
        readonly ICCAvenueServices _ccAvenueService;

        public CC_AvenueApiController(ICCAvenueServices ccAvenueServices)
        {
            _ccAvenueService = ccAvenueServices;
        }

        [System.Web.Http.HttpPost("create")]
        public HttpResponseMessage CreateCCAvenue(CCAvenueViewModel ccAvenueViewModel)
        {
            // TransactionStatus transactionStatus;
            var results = new CCAvenueValidation().Validate(ccAvenueViewModel);
            if (!results.IsValid)
            {
                ccAvenueViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                ccAvenueViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                ccAvenueViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, ccAvenueViewModel);
                return badResponse;
            }
            try
            {
                var ccavenueBo = BuiltCCAvenueBo(ccAvenueViewModel);

                var jsonResult = JsonConvert.SerializeObject(_ccAvenueService.AddCCAvenue(ccavenueBo));
                if (jsonResult != null)
                {
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    ccAvenueViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                    ccAvenueViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                    ccAvenueViewModel.Status = false;
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, ccAvenueViewModel);
                    return badResponse;
                }
                          }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }


        [System.Web.Http.HttpGet("Bind")]
        public HttpResponseMessage Bind()
        {
            try
            {
                List<Object> CCAvenueList = _ccAvenueService.Bind();

                var jsonResult = JsonConvert.SerializeObject(CCAvenueList);

                if (jsonResult != null)
                {
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }



        [System.Web.Http.HttpPost("GetRoomById")]
        [System.Web.Http.HttpGet("GetRoomById")]
        public HttpResponseMessage GetCCAvenue(int roomtype_Id)
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_ccAvenueService.GetCCAvenueById(roomtype_Id));
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

        private CCAvenueBo BuiltCCAvenueBo(CCAvenueViewModel ccAvenueViewModel)
        {
            return (CCAvenueBo)new CCAvenueBo().InjectFrom(ccAvenueViewModel);
        }

        [System.Web.Http.HttpPost("update")]
        public HttpResponseMessage UpdateCCAvenue(CCAvenueViewModel ccAvenueViewModel)
        {
            var results = new CCAvenueValidation().Validate(ccAvenueViewModel);
            if (!results.IsValid)
            {
                ccAvenueViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                ccAvenueViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                ccAvenueViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, ccAvenueViewModel);
                return badResponse;
            }
            TransactionStatus transactionStatus;
            try
            {
                var roomTypeBo = BuiltCCAvenueBo(ccAvenueViewModel);
                transactionStatus = _ccAvenueService.EditCCAvenue(roomTypeBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(ccAvenueViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully Updated to database");

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
        
    }
}