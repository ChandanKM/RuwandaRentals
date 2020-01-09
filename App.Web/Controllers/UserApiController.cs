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

    [RoutePrefix("api/users")]
    public class UserApiController : ApiController
    {
        readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Bind")]
        public HttpResponseMessage Bind()
        {
            List<Object> Userlist = _userService.Bind();

            var jsonResult = JsonConvert.SerializeObject(Userlist);

            if (jsonResult != null)
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                return response;
            }
            return null;


        }


        [HttpGet("Edit")]
        public List<Object> Edit(int Id)
        {

            List<Object> UserlistbyId = _userService.Edit(Id);
            return UserlistbyId;

        }
        [HttpDelete("Editd")]

        public HttpResponseMessage Delete(UserViewModel userViewModel)
        {


            TransactionStatus transactionStatus;
            var results = new UserValidation().Validate(userViewModel);

            if (!results.IsValid)
            {
                userViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                userViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                userViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, userViewModel);
                return badResponse;
            }
            try
            {
                var userBo = BuiltUserBo1(userViewModel);
                transactionStatus = _userService.DeleteUser(userBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(userViewModel));
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

        [HttpPost("Edit")]

        public HttpResponseMessage EditUser(UserViewModel userViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new UserValidation().Validate(userViewModel);

            if (!results.IsValid)
            {
                userViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                userViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                userViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, userViewModel);
                return badResponse;
            }
            try
            {
                var userBo = BuiltUserBo1(userViewModel);
                transactionStatus = _userService.EditUser(userBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(userViewModel));
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

        [HttpPost("create")]
        public HttpResponseMessage CreateUser(UserViewModel userViewModel)
        {
            TransactionStatus transactionStatus;
            var results = new UserValidation().Validate(userViewModel);

            if (!results.IsValid)
            {
                userViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                userViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                userViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, userViewModel);
                return badResponse;
            }
            try
            {
                var userBo = BuiltUserBo(userViewModel);
                transactionStatus = _userService.CreateUser(userBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(userViewModel));
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
        private UserBo BuiltUserBo(UserViewModel userViewModel)
        {
            return (UserBo)new UserBo().InjectFrom(userViewModel);
        }
        private UserBo1 BuiltUserBo1(UserViewModel userViewModel)
        {
            return (UserBo1)new UserBo1().InjectFrom(userViewModel);
        }
    }
}
