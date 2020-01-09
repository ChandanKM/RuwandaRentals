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

namespace App.Web.Controllers
{

    [RoutePrefix("api/city")]
    public class CityrApiController : ApiController
    {
        readonly ICityService _cityService;
        //readonly IUserService _userService;

        public CityrApiController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("Bind")]
        public List<Object> Bind()
        {

            List<Object> Citylist = _cityService.Bind();
            return Citylist;

        }
        [HttpGet("Edit")]
        public List<Object> Edit(int Id)
        {

            List<Object> CitylistbyId = _cityService.Edit(Id);
            return CitylistbyId;

        }
      

        [HttpPost("Edit")]

        public HttpResponseMessage EditCity(CityViewModel cityViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new CityValidation1().Validate(cityViewModel);

            if (!results.IsValid)
            {
                cityViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                cityViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                cityViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, cityViewModel);
                return badResponse;
            }

            var cityBo = BuiltCityBo1(cityViewModel);
            transactionStatus = _cityService.EditCity(cityBo);

            if (transactionStatus.Status == false)
            {
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(cityViewModel));
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

        [HttpPost("create")]
        public HttpResponseMessage CreateCity(CityViewModel cityViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new CityValidation1().Validate(cityViewModel);

            if (!results.IsValid)
            {
                cityViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                cityViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                cityViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, cityViewModel);
                return badResponse;
            }

            var cityBo = BuiltCityBo(cityViewModel);
            transactionStatus = _cityService.CreateCity(cityBo);

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
        private CityBo BuiltCityBo(CityViewModel cityViewModel)
        {
            return (CityBo)new CityBo().InjectFrom(cityViewModel);
        }
        private CityBo1 BuiltCityBo1(CityViewModel cityViewModel)
        {
            return (CityBo1)new CityBo1().InjectFrom(cityViewModel);
        }


    }
}