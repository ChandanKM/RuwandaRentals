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

    [RoutePrefix("api/Promotion")]
    public class PromotionApiController : ApiController
    {
        readonly IPromotionServices _promotionService;
       
        public PromotionApiController(IPromotionServices promotionServices)
        {
            _promotionService = promotionServices;
        }

        [HttpPost("create")]
        public HttpResponseMessage AddPromotion(PromotionViewModel promotionViewModel)
        {
            TransactionStatus transactionStatus;
            try
            {
                var promotion = BuiltPromotionBo(promotionViewModel);
                transactionStatus = _promotionService.AddPromotion(promotion);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(promotionViewModel));
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
        public HttpResponseMessage GetPromotion()
        {
            var jsonResult = JsonConvert.SerializeObject(_promotionService.Bind());

            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        }

        [HttpPost("Suspend")]
        public HttpResponseMessage SuspendPromotion(PromotionViewModel promo)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _promotionService.SuspendPromotion(promo.Promo_Id);
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
        
        public PromotionBo BuiltPromotionBo(PromotionViewModel promotionVm)
        {
            return (PromotionBo)new PromotionBo().InjectFrom(promotionVm);
        }
    }
}
