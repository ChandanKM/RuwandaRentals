using App.BusinessObject;
using App.Common;
using App.UIServices;
using App.UIServices.InterfaceServices;
using App.Web.ModelValidation;
using App.Web.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Omu.ValueInjecter;

namespace App.Web.Controllers
{
    [RoutePrefix("api/property")]
    public class PropertyApiController : ApiController
    {
        readonly IPropertyService _propertyService;

        public PropertyApiController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        #region Common
        [HttpGet("allcities")]
        public HttpResponseMessage GetCity()
        {
            var list = _propertyService.GetCity();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }



        [HttpGet("allfacilitytype")]
        public HttpResponseMessage GetFacilityType()
        {
            var list = _propertyService.GetFacilityType();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }

        [HttpGet("allfacilityname")]
        public HttpResponseMessage GetFacilityName()
        {
            var list = _propertyService.GetFacilityName();
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
        }
        #endregion

        #region Property
        [HttpGet("Bind")]
        public List<Object> Bind(int VendId)
        {    

            List<Object> Propertylist = _propertyService.Bind(VendId);
            return Propertylist;
        }

        [HttpPost("create")]
        public HttpResponseMessage Create(PropertyViewModel propertyViewModel)
        {
            var results = new PropertyValidation().Validate(propertyViewModel);
            try
            {
                if (!results.IsValid)
                {
                    propertyViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                    propertyViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                    propertyViewModel.Status = false;
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, propertyViewModel);
                    return badResponse;
                }

                var propertyBo = BuiltPropertyBo(propertyViewModel);
                String result = _propertyService.CreateProperty(propertyBo);

                var badResponse1 = Request.CreateResponse(HttpStatusCode.OK, result);
                return badResponse1;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("Edit")]
        public HttpResponseMessage Edit(PropertyViewModel propertyViewModel)
        {

            TransactionStatus transactionStatus;
            var results = new PropertyValidation().Validate(propertyViewModel);

            if (!results.IsValid)
            {
                propertyViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                propertyViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                propertyViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, propertyViewModel);
                return badResponse;
            }
            //propertyViewModel.Prop_Expires_on = DateTime.UtcNow;
            //propertyViewModel.Prop_Approved_on = DateTime.UtcNow;
            try
            {
                var userBo = BuiltPropertyBo(propertyViewModel);
                transactionStatus = _propertyService.Edit(userBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully updated.");

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

        [HttpGet("Edit")]
        public HttpResponseMessage Edit(string Id)
        {
            try
            {
                var list = _propertyService.Edit(Id);
                var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
                return badResponse;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("DeteteProperty")]
        public HttpResponseMessage Delete(PropertyViewModel propertyViewModel)
        {


            TransactionStatus transactionStatus;

            propertyViewModel.Prop_Expires_on = DateTime.UtcNow;
            propertyViewModel.Prop_Approved_on = DateTime.UtcNow;
            try
            {
                var userBo = BuiltPropertyBo(propertyViewModel);
                transactionStatus = _propertyService.DeleteProperty(userBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully dissabled to database");

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
        #endregion

        #region Bank Details

        [HttpPost("createBank")]
        public HttpResponseMessage createBank(PropertyViewModel propertyViewModel)
        {


            var results = new PropertyValidationBanck().Validate(propertyViewModel);

            if (!results.IsValid)
            {
                propertyViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                propertyViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                propertyViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, propertyViewModel);
                return badResponse;
            }
            try
            {
                var propertyBo = BuiltPropertyBo(propertyViewModel);



                string result = _propertyService.CreateBankDetails(propertyBo);
                var badResponse1 = Request.CreateResponse(HttpStatusCode.OK, result);
                return badResponse1;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }

        [HttpPost("EditBank")]
        public HttpResponseMessage EditBank(PropertyViewModel propertyViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new PropertyValidationBanck().Validate(propertyViewModel);

            if (!results.IsValid)
            {
                propertyViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                propertyViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                propertyViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, propertyViewModel);
                return badResponse;
            }
            try
            {
                var bankBo = BuiltPropertyBo(propertyViewModel);
                transactionStatus = _propertyService.EditBank(bankBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully updated.");

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

        [HttpGet("EditBankDetails")]
        public HttpResponseMessage EditBankDetails(string Id)
        {
            try
            {
                var list = _propertyService.EditBankDetails(Id);
                var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
                return badResponse;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }
        //
        #endregion

        #region Facility
        [HttpGet("BindFacility")]
        public List<Object> BindFacility(string Id)
        {
            try
            {
                List<Object> list = _propertyService.BindFacility(Id);
                return list;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpPost("createfacility")]
        public HttpResponseMessage CreateFacility(PropertyViewModel propertyViewModel)
        {
            TransactionStatus transactionStatus;
            try
            {
                var propertyBo = BuiltPropertyBo(propertyViewModel);
                transactionStatus = _propertyService.CreateFacility(propertyBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
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

        [HttpPost("DeteteFacility")]
        public HttpResponseMessage DeleteFacility(PropertyViewModel propertyViewModel)
        {
            TransactionStatus transactionStatus;
            try
            {
                var propertyBo = BuiltPropertyBo(propertyViewModel);
                transactionStatus = _propertyService.DeleteFacility(propertyBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
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
        #endregion

        #region Policy
        [HttpGet("Bindpolicy")]
        public HttpResponseMessage BindPolicy(int PropId, int VendId)
        {
            try
            {
                var list = _propertyService.BindPolicy(PropId, VendId);
                var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
                return badResponse;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }


        [HttpGet("BindRoompolicy")]
        public HttpResponseMessage BindRoomPolicy(int PropId, int VendId,int RoomId)
        {
            try
            {
                var list = _propertyService.BindRoomPolicy(PropId, VendId,RoomId);
                var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
                return badResponse;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }
        [HttpPost("DeteteEvent")]
        public HttpResponseMessage DeteteEvent(PropertyViewModel propertyViewModel)
        {
            TransactionStatus transactionStatus;
            try
            {
                var propertyBo = BuiltPropertyBo(propertyViewModel);
                transactionStatus = _propertyService.DeteteEvent(propertyBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
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

        [HttpPost("createRoomPolicy")]
        public HttpResponseMessage createRoomPolicy(PropertyViewModel propertyViewModel)
        {

            if (propertyViewModel.Policy_Id != 0)
            {
                TransactionStatus transactionStatus;
                try
                {
                    var propertyBo = BuiltPropertyBo(propertyViewModel);
                    transactionStatus = _propertyService.EditPolicy(propertyBo);

                    if (transactionStatus.Status == false)
                    {
                        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
                        return badResponse;
                    }
                    else
                    {
                        transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                        transactionStatus.ReturnMessage.Add("Record successfully updated to database");

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
            else
            {

                var results = new PropertyPolicyValidation().Validate(propertyViewModel);

                if (!results.IsValid)
                {
                    propertyViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                    propertyViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                    propertyViewModel.Status = false;
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, propertyViewModel);
                    return badResponse;
                }
                TransactionStatus transactionStatus;
                try
                {
                    var propertyBo = BuiltPropertyBo(propertyViewModel);
                    transactionStatus = _propertyService.createRoomPolicy(propertyBo);

                    if (transactionStatus.Status == false)
                    {
                        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
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
        }

        [HttpPost("createPolicy")]
        public HttpResponseMessage createPolicy(PropertyViewModel propertyViewModel)
        {

            if (propertyViewModel.Policy_Id != 0)
            {
                TransactionStatus transactionStatus;
                try
                {
                    var propertyBo = BuiltPropertyBo(propertyViewModel);
                    transactionStatus = _propertyService.EditPolicy(propertyBo);

                    if (transactionStatus.Status == false)
                    {
                        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
                        return badResponse;
                    }
                    else
                    {
                        transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                        transactionStatus.ReturnMessage.Add("Record successfully updated to database");

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
            else
            {

                var results = new PropertyPolicyValidation().Validate(propertyViewModel);

                if (!results.IsValid)
                {
                    propertyViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                    propertyViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                    propertyViewModel.Status = false;
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, propertyViewModel);
                    return badResponse;
                }
                TransactionStatus transactionStatus;
                try
                {
                    var propertyBo = BuiltPropertyBo(propertyViewModel);
                    transactionStatus = _propertyService.createPolicy(propertyBo);

                    if (transactionStatus.Status == false)
                    {
                        var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
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
        }

        [HttpPost("DeletePolicy")]
        public HttpResponseMessage DetetePolicy(PropertyViewModel propertyViewModel)
        {
            TransactionStatus transactionStatus;
            try
            {
                var propertyBo = BuiltPropertyBo(propertyViewModel);
                transactionStatus = _propertyService.DetetePolicy(propertyBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
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


        #endregion

        #region PropertyImage
        [HttpGet("BindImage")]
        public HttpResponseMessage BindImage(string Id)
        {
            try
            {
                var list = _propertyService.BindImage(Id);
                var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
                return badResponse;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        [HttpGet("UpdateImageStatus")]
        public HttpResponseMessage UpdateImageFlag(int Id, string flag)
        {
            TransactionStatus transactionStatus;
            flag = flag == "true" ? "false" : "true";
            try
            {

                transactionStatus = _propertyService.UpdateImageFlag(Id, flag);
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
                    transactionStatus.ReturnMessage.Add("Successfully Updated");

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

        [HttpGet("SetDefaultImage")]
        public HttpResponseMessage SetDefaultImage(int PropId, int ImageId)
        {
            TransactionStatus transactionStatus;        
            try
            {
                transactionStatus = _propertyService.SetDefaultImage(PropId, ImageId);
                if (transactionStatus.Status == false)
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Error.ToString();
                    transactionStatus.ReturnMessage.Add("Image Not Set. Retry");

                    var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus.ReturnMessage);

                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("This Image has been set as Default Image");

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

        #endregion

        [HttpPost("DeteteImage")]
        public HttpResponseMessage DeteteImage(PropertyViewModel propertyViewModel)
        {
            TransactionStatus transactionStatus;
            try
            {
                var propertyBo = BuiltPropertyBo(propertyViewModel);
                transactionStatus = _propertyService.DeteteImage(propertyBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record  deleted to database");

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
        private PropertyBo BuiltPropertyBo(PropertyViewModel propertyViewModel)
        {
            return (PropertyBo)new PropertyBo().InjectFrom(propertyViewModel);
        }

        #region Facility


        [HttpGet("BindFacility")]
        public List<Object> BindFacility(int prop_id)
        {
            try
            {
                List<Object> roomlist = _propertyService.BindFacilityimage(prop_id);



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
        public HttpResponseMessage ActivateFacility(PropertyViewModel propertyViewModel)
        {

            TransactionStatus transactionStatus;
            try
            {
                var RoomFacilityBo = BuiltRoomFacilityEditBo(propertyViewModel);
                transactionStatus = _propertyService.ActivateFacility(RoomFacilityBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(propertyViewModel));
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



        private RoomFacilityBo BuiltRoomFacilityBo(PropertyViewModel propertyViewModel)
        {
            return (RoomFacilityBo)new RoomFacilityBo().InjectFrom(propertyViewModel);
        }
        private RoomFacilityEditBo BuiltRoomFacilityEditBo(PropertyViewModel propertyViewModel)
        {
            return (RoomFacilityEditBo)new RoomFacilityEditBo().InjectFrom(propertyViewModel);
        }

        #endregion

        [HttpGet("GetAutoCompleteSearch")]
        public HttpResponseMessage GetAllCities(string terms)
        {

            var autocompleteText = _propertyService.GetAutoCompleteLocationWithId(terms);
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, autocompleteText);

            return badResponse;
        }
        [HttpGet("PropertyAutoCompleteSearch")]
        public HttpResponseMessage PropertyAutoCompleteSearch(string terms)
        {

            var autocompleteText = _propertyService.PropertyAutoCompleteSearch(terms);
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, autocompleteText);

            return badResponse;
        }
    }
}
