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

    [RoutePrefix("api/Subscribe")]
    public class SubscribeApiController : ApiController
    {
        readonly ISubscribeServices _subscribeService;

        public SubscribeApiController(ISubscribeServices subscribeService)
        {
            _subscribeService = subscribeService;
        }

        [HttpPost("create")]
        public HttpResponseMessage AddSubscribe(SubscribeViewModel subscribeViewModel)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            var results = new SubscribeValidation().Validate(subscribeViewModel);
            if (!results.IsValid)
            {
                subscribeViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                subscribeViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                subscribeViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, subscribeViewModel);
                return badResponse;
            }
            try
            {
                var subscribe = BuiltSubscribeBo(subscribeViewModel);
                transactionStatus = _subscribeService.AddSubscribe(subscribe);

                //if (transactionStatus.Status == false)
                //{
                //    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                //    transactionStatus.ReturnMessage.Add("Already Subscribed With This Email ");
                //    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, transactionStatus);
                //    return badResponse;
                //}
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


        public SubscribeBo BuiltSubscribeBo(SubscribeViewModel subscribeVm)
        {
            return (SubscribeBo)new SubscribeBo().InjectFrom(subscribeVm);
        }

    }
}
