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
namespace App.Web.Controllers
{
    [RoutePrefix("api/ManageLocation")]
    public class ManageLocationApiController : ApiController
    {
        readonly IManageLocationServices _locationService;
        public ManageLocationApiController(IManageLocationServices locationService)
        {
            _locationService = locationService;
        }

        [HttpPost("create")]
        public HttpResponseMessage AddLocation(ManageLocationViewModel locationVM)
        {
         
            var results = new ManageLocationValidation().Validate(locationVM);
            if (!results.IsValid)
            {
                locationVM.Errors = GenerateErrorMessage.Built(results.Errors);
                locationVM.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                locationVM.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, locationVM);
                return badResponse;
            }
            try
            {
                var location = BuiltManageLocationBo(locationVM);

                var jsonResult = JsonConvert.SerializeObject(_locationService.AddLocation(location));
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

        [HttpPost("update")]
        public HttpResponseMessage UpdateLocation(ManageLocationViewModel locationVM)
        {
            TransactionStatus transactionStatus;
            var results = new ManageLocationValidation().Validate(locationVM);
            if (!results.IsValid)
            {
                locationVM.Errors = GenerateErrorMessage.Built(results.Errors);
                locationVM.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                locationVM.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, locationVM);
                return badResponse;
            }
            try
            {
                var location = BuiltManageLocationBo(locationVM);
                transactionStatus = _locationService.UpdateLocation(location);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(locationVM));
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

        [HttpGet("Get")]
        public HttpResponseMessage GetLocation()
        {
            var jsonResult = JsonConvert.SerializeObject(_locationService.GetAllLocation());
            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        }

        [HttpPost("delete")]
        public HttpResponseMessage SuspendUserProfile(ManageLocationViewModel locationVM)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _locationService.Delete(locationVM.Id);
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
                    transactionStatus.ReturnMessage.Add("Location successfully Deleted");

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


        private ManageLocationBo BuiltManageLocationBo(ManageLocationViewModel locationVM)
        {
            return (ManageLocationBo)new ManageLocationBo().InjectFrom(locationVM);
        }

    }
}