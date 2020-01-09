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

    [RoutePrefix("api/Loyalty")]
    public class LoyaltyApiController : ApiController
    {
        readonly ILoyaltyServices _loyaltyService;
     
        public LoyaltyApiController(ILoyaltyServices loyaltyService)
        {
            _loyaltyService = loyaltyService;
        }

        [HttpPost("create")]
        public HttpResponseMessage AddLoyalty(LoyaltyViewModel loyaltyViewModel)
        {
            TransactionStatus transactionStatus;
            try
            {
                var loyalty = BuiltLoyaltyBo(loyaltyViewModel);
                transactionStatus = _loyaltyService.AddLoyalty(loyalty);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(loyaltyViewModel));
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

        [HttpGet("Bind")]
        public HttpResponseMessage GetLoyalty()
        {
            var jsonResult = JsonConvert.SerializeObject(_loyaltyService.Bind());

            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        }

        [HttpPost("Suspend")]
        public HttpResponseMessage SuspendLoyalty(LoyaltyViewModel loyalty)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _loyaltyService.SuspendLoyalty(loyalty.Loyal_Id);
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

        public LoyaltyBo BuiltLoyaltyBo(LoyaltyViewModel loyaltyVm)
        {
            return (LoyaltyBo)new LoyaltyBo().InjectFrom(loyaltyVm);
        }

    }
}
