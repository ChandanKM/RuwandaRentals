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

    [RoutePrefix("api/RoomType")]
    public class RoomTypeApiController : ApiController
    {
        readonly IRoomTypeServices _roomTypeService;

        public RoomTypeApiController(IRoomTypeServices roomTypeServices)
        {
            _roomTypeService = roomTypeServices;
        }

        [HttpPost("create")]
        public HttpResponseMessage CreateRoomType(RoomTypeViewModel roomTypeViewModel)
        {
            // TransactionStatus transactionStatus;
            var results = new RoomTypeValidation().Validate(roomTypeViewModel);
            if (!results.IsValid)
            {
                roomTypeViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                roomTypeViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                roomTypeViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roomTypeViewModel);
                return badResponse;
            }
            try
            {
                var roomTypeBo = BuiltRoomTypeBo(roomTypeViewModel);

                var jsonResult = JsonConvert.SerializeObject(_roomTypeService.AddRoomType(roomTypeBo));
                if (jsonResult != null)
                {
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    roomTypeViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                    roomTypeViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                    roomTypeViewModel.Status = false;
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roomTypeViewModel);
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
        public HttpResponseMessage Bind()
        {
            try
            {
                List<Object> RoomTypeList = _roomTypeService.Bind();

                var jsonResult = JsonConvert.SerializeObject(RoomTypeList);

                if (jsonResult != null)
                {
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }



        [HttpPost("GetRoomById")]
        [HttpGet("GetRoomById")]
        public HttpResponseMessage GetRoomType(int roomtype_Id)
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_roomTypeService.GetRoomTypeById(roomtype_Id));
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

        private RoomTypeBo BuiltRoomTypeBo(RoomTypeViewModel roomTypeViewModel)
        {
            return (RoomTypeBo)new RoomTypeBo().InjectFrom(roomTypeViewModel);
        }

        [HttpPost("update")]
        public HttpResponseMessage UpdateRoomType(RoomTypeViewModel roomTypeViewModel)
        {
            var results = new RoomTypeValidation().Validate(roomTypeViewModel);
            if (!results.IsValid)
            {
                roomTypeViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                roomTypeViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                roomTypeViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roomTypeViewModel);
                return badResponse;
            }
            TransactionStatus transactionStatus;
            try
            {
                var roomTypeBo = BuiltRoomTypeBo(roomTypeViewModel);
                transactionStatus = _roomTypeService.EditRoomType(roomTypeBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(roomTypeViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully Updated to database");

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

        [HttpPost("Suspend")]
        public HttpResponseMessage SuspendRoomTypeById(RoomTypeViewModel roomtype)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _roomTypeService.SuspendRoomType(roomtype.Room_TypeId);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Warning.ToString();
                    transactionStatus.ReturnMessage.Add("Room Not Suspended");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Room successfully Suspended");

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

    }
}
