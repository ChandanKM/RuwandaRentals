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
namespace App.Web.Controllers
{
    [RoutePrefix("api/location")]
    public class LocationApiController : ApiController
    {
        // GET: LocationApi
        readonly ILocationService _locationService;

        public LocationApiController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost("create")]
        public HttpResponseMessage CreateLocation(LocationViewModel locationViewModel)
        {
            var results = new LocationValidation().Validate(locationViewModel);

            if (!results.IsValid)
            {
                locationViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                locationViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                locationViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, locationViewModel);
                return badResponse;
            }
            try
            {
                var locationBo = BuiltLocationBo(locationViewModel);
                TransactionStatus transactionStatus = _locationService.CreateLocation(locationBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(locationViewModel));
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



        [HttpPost("EditLocation")]
        public HttpResponseMessage EditLocation(LocationViewModel locationViewModel)
        {
            var results = new LocationValidation().Validate(locationViewModel);

            if (!results.IsValid)
            {
                locationViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                locationViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                locationViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, locationViewModel);
                return badResponse;
            }
            try
            {
                var locationBo = BindEditLocationBo(locationViewModel);

                TransactionStatus transactionStatus = _locationService.EditLocation(locationBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(locationViewModel));
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


        private static LocationBo BuiltLocationBo(LocationViewModel locationViewModel)
        {
            return (LocationBo)new LocationBo().InjectFrom(locationViewModel);
        }

        private static EditLocationBo BindEditLocationBo(LocationViewModel locationViewModel)
        {
            return (EditLocationBo)new EditLocationBo().InjectFrom(locationViewModel);
        }

        [HttpGet("Bind")]
        public List<Object> Bind()
        {
            List<Object> locationlist = _locationService.BindLocation();
            return locationlist;

        }
        [HttpGet("Edit")]
        public List<Object> Edit(string Location_Id)
        {
            List<Object> locationlistbyId = _locationService.Edit(Location_Id);
            return locationlistbyId;

        }
    }
}