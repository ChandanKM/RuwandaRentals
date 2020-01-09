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
using System.Xml.Serialization;
using System.IO;
using System.Text;



namespace App.Web.Controllers
{

    [RoutePrefix("api/CityMaster")]
    public class CityMasterApiController : ApiController
    {
        readonly ICityMasterService _cityService;

        public CityMasterApiController(ICityMasterService cityService)
        {
            _cityService = cityService;
        }
        [HttpGet("City")]
        public HttpResponseMessage Bind()
        {
           // List<Object> Userlist = _cityService.Bind();

            var jsonResult = JsonConvert.SerializeObject(_cityService.Bind());

            if (jsonResult != null)
            {

                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);


        }
        [HttpPost("create")]
        public HttpResponseMessage CreateCity(CityMasterViewModel cityViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new CityValidation().Validate(cityViewModel);

            if (!results.IsValid)
            {
                cityViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                cityViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                cityViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, cityViewModel);
                return badResponse;
            }
            try{
            var userBo = BuiltUserBo(cityViewModel);
            transactionStatus = _cityService.AddCityMaster(userBo);

            if (transactionStatus.Status == false)
            {
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(cityViewModel));
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

            catch(Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }
        private CityMasterBo BuiltUserBo(CityMasterViewModel cityViewModel)
        {
            return (CityMasterBo)new CityMasterBo().InjectFrom(cityViewModel);
        }
    
    }
}
