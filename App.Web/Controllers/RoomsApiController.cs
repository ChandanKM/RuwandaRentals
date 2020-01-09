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
    [RoutePrefix("api/rooms")]
    public class RoomsApiController : ApiController
    {
        // GET: RoomsApi
        readonly IRoomsService _roomsService;
       // private RoomViewModel model;
        readonly IVendorService _vedorservices;

        public RoomsApiController(IRoomsService roomsService,IVendorService services)
        {
            _roomsService = roomsService;
            _vedorservices = services;
        }

       
        #region Room
        [HttpGet("GetAutoCompleteSearch")]
        public HttpResponseMessage GetAllRoomType(string terms)
        {

            var autocompleteText = _roomsService.GetAutoCompleteRoom(terms);
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, autocompleteText);

            return badResponse;
        }
        //Bind Room data To GridView
        [HttpGet("Bind")]
        public List<Object> Bind(int Prop_Id)
        {
            try
            {
                List<Object> roomlist = _roomsService.Bind(Prop_Id);

                var jsonResult = JsonConvert.SerializeObject(roomlist);


                return roomlist;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //Get Room Data To Edit 
        [HttpGet("Edit")]
        public List<Object> Edit(int Id)
        {
            try
            {

                List<Object> roomlistbyId = _roomsService.Edit(Id);
                return roomlistbyId;

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //Bind RoomType data to Dropdown 
        [HttpGet("allroomtypes")]
        public HttpResponseMessage GetRoomType()
        {
            try
            {
                var roomtype = _roomsService.GetRoomType();
                var badResponse = Request.CreateResponse(HttpStatusCode.OK, roomtype);
                return badResponse;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }
              

        //Create Room Data
        [HttpPost("create")]
        public HttpResponseMessage CreateRoom(RoomViewModel roomViewModel)
        {

           // TransactionStatus transactionStatus;
            var results = new RoomValidation().Validate(roomViewModel);

            if (!results.IsValid)
            {
                roomViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                roomViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                roomViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roomViewModel);
                return badResponse;
            }
            try
            {

                var roomBo = BuiltroomsBo(roomViewModel);
                String result = _roomsService.CreateRooms(roomBo);
                var badResponse1 = Request.CreateResponse(HttpStatusCode.OK, result);
                try
                {
                    _vedorservices.ExecuteAddRoomTimer();  // start the Room inventory Timer
                }
                catch (Exception exc)
                { ApplicationErrorLogServices.AppException(exc); }

                return badResponse1;
               
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        //Update Room Data
        [HttpPost("EditRoomdetail")]
        public HttpResponseMessage EditRooms(RoomViewModel roomViewModel)
        {

            TransactionStatus transactionStatus;
            var results = new RoomValidation().Validate(roomViewModel);

            if (!results.IsValid)
            {
               roomViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                roomViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                roomViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roomViewModel);
                return badResponse;
            }
            try
            {
                var roomBo = BuiltroomsBo1(roomViewModel);
                transactionStatus = _roomsService.EditRooms(roomBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(roomViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully updated");

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
         

        //Suspend Room Data
        [HttpPost("Suspend")]
        public HttpResponseMessage SuspendRoomTypeById(RoomViewModel roomviewmodel)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _roomsService.SuspendRoom(roomviewmodel.Room_Id);
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


        //Delete Room Data
        [HttpDelete("Editd")]
        public HttpResponseMessage Delete(RoomViewModel roomViewModel)
        {

            try
            {
                TransactionStatus transactionStatus;
                var vendorBo = BuiltroomsBo1(roomViewModel);
                transactionStatus = _roomsService.DeleteRooms(vendorBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(vendorBo));
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

        



        [HttpGet("updaterackPrice")]
        public HttpResponseMessage updaterackPrice(int Inv_Id, int race_price)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _roomsService.UpdateRackPrice(Inv_Id, race_price);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString();
                    transactionStatus.ReturnMessage.Add("Not upadated");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus.ReturnMessage);

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




        private RoomsBo BuiltroomsBo(RoomViewModel RoomViewModel)
        {
            return (RoomsBo)new RoomsBo().InjectFrom(RoomViewModel);
        }
        private RoomsEditBo BuiltroomsBo1(RoomViewModel RoomViewModel)
        {
            return (RoomsEditBo)new RoomsEditBo().InjectFrom(RoomViewModel);
        }

        #endregion

        #region POLICIES

        // Bind Policy data to gridview
        [HttpGet("BindPolicy")]
        public List<Object> BindPolicies(int Prop_Id, int Vendor_Id)
        {
            try
            {
                List<Object> roomlist = _roomsService.Bindpolicies(Prop_Id, Vendor_Id);

                var jsonResult = JsonConvert.SerializeObject(roomlist);


                return roomlist;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        //Create Policy without using ko
        public RoomsApiController(RoomViewModel model)
        {
            try
            {

                var transactionStatus = new TransactionStatus();
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                int value = 0;
                CemexDb con = new CemexDb();
                SqlParameter[] Params = 
            { 	
            
                new SqlParameter("@Prop_Id",value ),//0 model.Prop_Id
                new SqlParameter("@Room_Id",value ),//1 model.Room_Id
                new SqlParameter("@Vndr_Id", value),//2model.Vndr_Id
                new SqlParameter("@Policy_Name", model.Policy_Name),//3 
                new SqlParameter("@Policy_Descr", model.Policy_Descr),//4   
                new SqlParameter("@opReturnValue", 1)//5


            };

                Params[5].Direction = ParameterDirection.Output;
                DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPolicies", Params);
                string test = Params[5].Value.ToString();

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
            }
        }

        //Suspend Policy
        [HttpPost("Suspendpolicy")]
        public HttpResponseMessage SuspendPolicyTypeById(RoomViewModel policyviewmodel)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _roomsService.Suspendpolicy(policyviewmodel.Policy_Id);
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

        //Create Policy
        [HttpPost("Createpolicies")]
        public HttpResponseMessage Createpolicies(RoomViewModel RoomViewModel)
        {

            TransactionStatus transactionStatus;
            var results = new RoomValidation().Validate(RoomViewModel);

            if (!results.IsValid)
            {
                RoomViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                RoomViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                RoomViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, RoomViewModel);
                return badResponse;
            }
            try
            {

                var policyBo = BuiltpolicyBo(RoomViewModel);
                transactionStatus = _roomsService.Createpolicies(policyBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(RoomViewModel));
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

        //Edit Policy
        [HttpGet("Editpolicy")]
        public List<Object> Editpoliciesbyid(int Id)
        {
            try
            {

                List<Object> roomlistbyId = _roomsService.Editpoliciesbyid(Id);
                return roomlistbyId;

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }


        [HttpPost("Updatepolicy")]
        public HttpResponseMessage Editpolicies(RoomViewModel RoomViewModel)
        {

            TransactionStatus transactionStatus;
            try
            {
                var policyBo = BuiltpolicyBo1(RoomViewModel);
                transactionStatus = _roomsService.Editpolicies(policyBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(RoomViewModel));
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

        private PoliciesBo BuiltpolicyBo(RoomViewModel RoomViewModel)
        {
            return (PoliciesBo)new PoliciesBo().InjectFrom(RoomViewModel);
        }

        private PoliciesEditBo BuiltpolicyBo1(RoomViewModel RoomViewModel)
        {
            return (PoliciesEditBo)new PoliciesEditBo().InjectFrom(RoomViewModel);
        }
        #endregion


        #region Facility


        [HttpGet("BindFacility")]
        public List<Object> BindFacility(int Room_id)
        {
            try
            {
                List<Object> roomlist = _roomsService.BindFacility(Room_id);

                

                var jsonResult = JsonConvert.SerializeObject(roomlist);


                return roomlist;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }


        [HttpPost("ActivateFacility")]
        public HttpResponseMessage ActivateFacility(RoomViewModel RoomViewModel)
        {

            TransactionStatus transactionStatus;
            try
            {
                var RoomFacilityBo = BuiltRoomFacilityEditBo(RoomViewModel);
                transactionStatus = _roomsService.ActivateFacility(RoomFacilityBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(RoomViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("");
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



        private RoomFacilityBo BuiltRoomFacilityBo(RoomViewModel RoomViewModel)
        {
            return (RoomFacilityBo)new RoomFacilityBo().InjectFrom(RoomViewModel);
        }
        private RoomFacilityEditBo BuiltRoomFacilityEditBo(RoomViewModel RoomViewModel)
        {
            return (RoomFacilityEditBo)new RoomFacilityEditBo().InjectFrom(RoomViewModel);
        }

        #endregion

    }
}