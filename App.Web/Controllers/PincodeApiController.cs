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
using App.UIServices.InterfaceServices;

namespace App.Web.Controllers
{
    [RoutePrefix("api/pincode")]
    public class PincodeApiController : ApiController
    {
        readonly IPincodeService _pincodeService;

        public PincodeApiController(IPincodeService pincodeService)
        {
            _pincodeService = pincodeService;
        }

        [HttpPost("create")]
        public HttpResponseMessage CreatePincode(PincodeViewModel pincodeViewModel)
        {
            var results = new PincodeValidation().Validate(pincodeViewModel);

            if (!results.IsValid)
            {
                pincodeViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                pincodeViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                pincodeViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, pincodeViewModel);
                return badResponse;
            }
            try
            {
                var pincodeBo = BuiltPincodeBo(pincodeViewModel);
                TransactionStatus transactionStatus = _pincodeService.CreatePincode(pincodeBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(pincodeViewModel));
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

        [HttpPost("EditPincode")]
        public HttpResponseMessage EditPincode(PincodeViewModel pincodeViewModel)
        {
            var results = new PincodeValidation().Validate(pincodeViewModel);

            if (!results.IsValid)
            {
                pincodeViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                pincodeViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                pincodeViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, pincodeViewModel);
                return badResponse;
            }
            try
            {
                var pincodeBo = BindEditPincodeBo(pincodeViewModel);
                TransactionStatus transactionStatus = _pincodeService.EditPincode(pincodeBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(pincodeViewModel));
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


        private static PincodeBo BuiltPincodeBo(PincodeViewModel pincodeViewModel)
        {
            return (PincodeBo)new PincodeBo().InjectFrom(pincodeViewModel);
        }

        private static EditPincodeBo BindEditPincodeBo(PincodeViewModel pincodeViewModel)
        {
            return (EditPincodeBo)new EditPincodeBo().InjectFrom(pincodeViewModel);
        }

        [HttpGet("Bind")]
        public List<Object> Bind()
        {
            List<Object> pincodelist = _pincodeService.BindPincode();
            return pincodelist;

        }
        [HttpGet("Edit")]
        public List<Object> Edit(string id)
        {
            List<Object> pincodelistbyId = _pincodeService.Edit(id);
            return pincodelistbyId;

        }

    }
}