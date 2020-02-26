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
    [RoutePrefix("api/facility")]
    public class FacilityApiController : ApiController
    {
         
         readonly IFacilityService _facilityService;

         public FacilityApiController(IFacilityService facilityService)
        {
            _facilityService = facilityService;
        }

        [HttpGet("Bind")]
        public HttpResponseMessage Bind()
        {
            List<Object> Facilitylist = _facilityService.BindFacility();

            var jsonResult = JsonConvert.SerializeObject(Facilitylist);

            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return null;


        }


        [HttpPost("create")]
        public HttpResponseMessage CreateFacility(FacilityViewModel facilityViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new FacilityValidation().Validate(facilityViewModel);

            if (!results.IsValid)
            {
                facilityViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                facilityViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                facilityViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, facilityViewModel);
                return badResponse;
            }
            try{
            var facilityBo = BuiltFacilityBo(facilityViewModel);
                //Sample Dir added for blob images.
                //Blob hardcode value.
                //Start
                facilityBo.Facility_Image_dir = "F:/New Dot net proj Pavan/LMKCloudBlob";
                //End
            transactionStatus = _facilityService.CreateFacility(facilityBo);

            if (transactionStatus.Status == false)
            {
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(facilityViewModel));
                return badResponse;
            }
            else
            {
                transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                transactionStatus.ReturnMessage.Add("Record successfully created");

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

        [HttpPost("DeteteFacilty")]
        public HttpResponseMessage Delete(FacilityViewModel facilityViewModel)
        {


            TransactionStatus transactionStatus;

         
            try
            {
                var facilityBo = BuiltFacilityBo(facilityViewModel);
                transactionStatus = _facilityService.DeleteFacility(facilityBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(facilityViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully deleted to database");

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
        [HttpGet("BindFacility")]
        public List<Object> BindFacility(string Id)
        {
            try
            {
                List<Object> list = _facilityService.Edit(Id);
                return list;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }
        [HttpPost("EditFacility")]
        public HttpResponseMessage EditFacility(FacilityViewModel facilityViewModel)
        {
            var results = new FacilityValidation().Validate(facilityViewModel);
            if (!results.IsValid)
            {
                facilityViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                facilityViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                facilityViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, facilityViewModel);
                return badResponse;
            }
            TransactionStatus transactionStatus;
            try
            {
                var bankBo = BuiltFacilityBo(facilityViewModel);
                transactionStatus = _facilityService.EditFacility(bankBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(facilityViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Facility Successfully updated.");

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

        private FacilityBo BuiltFacilityBo(FacilityViewModel facilityViewModel)
        {
            return (FacilityBo)new FacilityBo().InjectFrom(facilityViewModel);
        }
    }
}