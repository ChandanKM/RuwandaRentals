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
using App.UIServices.InterfaceServices;


namespace App.Web.Controllers
{
    [RoutePrefix("api/ApplicationException")]
    public class ApplicationExceptionApiController : ApiController
    {
        readonly IApplicationExceptionServices _appExceptionServices;

        public ApplicationExceptionApiController(IApplicationExceptionServices appexceptionservices)
        {
            _appExceptionServices = appexceptionservices;

        }

        [HttpPost("AddException")]
        public HttpResponseMessage AddException(ApplicationErrorLogViewModel appViewModel)
        {
            TransactionStatus transactionStatus;
            try
            {
                var apperrorlog = BuiltAppErrorLogBo(appViewModel);
                transactionStatus = _appExceptionServices.SaveException(apperrorlog);

                transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                transactionStatus.ReturnMessage.Add("Exception Caught,");

                var badResponse = Request.CreateResponse(HttpStatusCode.Created, transactionStatus);

                return badResponse;

            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }


        public ApplicationErrorLogBo BuiltAppErrorLogBo(ApplicationErrorLogViewModel appViewModel)
        {
            return (ApplicationErrorLogBo)new ApplicationErrorLogBo().InjectFrom(appViewModel);
        }

    }
}