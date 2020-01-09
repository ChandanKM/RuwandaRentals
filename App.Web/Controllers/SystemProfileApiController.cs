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
    [RoutePrefix("api/SystemProfile")]
    public class SystemProfileApiController : ApiController
    {
        readonly ISystemProfileServices _systemprofileService;

        public SystemProfileApiController(ISystemProfileServices systemprofileService)
        {
            _systemprofileService = systemprofileService;
        }

        #region SystemProfile
        [HttpPost("create")]
        public HttpResponseMessage AddProfile(SystemProfileViewModel systemprofileVm)
        {
            var results = new SystemProfileValidation().Validate(systemprofileVm);
            if (!results.IsValid)
            {
                systemprofileVm.Errors = GenerateErrorMessage.Built(results.Errors);
                systemprofileVm.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                systemprofileVm.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, systemprofileVm);
                return badResponse;
            }
            try
            {
                var systemprofile = BuiltSystemProfileBo(systemprofileVm);


                String result = _systemprofileService.AddSystemProfile(systemprofile);

                var badResponse1 = Request.CreateResponse(HttpStatusCode.OK, result);
                return badResponse1;


              
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("update")]
        public HttpResponseMessage UpdateProfile(SystemProfileViewModel systemprofileVm)
        {
            var results = new SystemProfileValidation1().Validate(systemprofileVm);
            if (!results.IsValid)
            {
                systemprofileVm.Errors = GenerateErrorMessage.Built(results.Errors);
                systemprofileVm.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                systemprofileVm.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, systemprofileVm);
                return badResponse;
            }
            TransactionStatus transactionStatus;
            try
            {
                var systemprofile = BuiltSystemProfileBo(systemprofileVm);
                transactionStatus = _systemprofileService.UpdateSystemProfile(systemprofile);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(systemprofileVm));
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


        [HttpPost("GetbyId")]
        public HttpResponseMessage GetProfileById(SystemProfileViewModel systemprofileVm)
        {
            var jsonResult = JsonConvert.SerializeObject(_systemprofileService.GetProfileById(systemprofileVm.Id));
            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound, jsonResult);
        }

        private SystemProfileBo BuiltSystemProfileBo(SystemProfileViewModel systemprofileVm)
        {
            return (SystemProfileBo)new SystemProfileBo().InjectFrom(systemprofileVm);
        }
        #endregion

    }
}