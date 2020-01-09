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
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using App.Domain;
using System.Net.Mail;
using App.UIServices.InterfaceServices;

namespace App.Web.Controllers
{
    [RoutePrefix("api/vendors")]
    public class VendorApiController : ApiController
    {
      

      
        // GET: VendorApi
        readonly IVendorService _vendorService;
         

        public VendorApiController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet("Bind")]
        public List<Object> Bind(int id)
        {
            try
            {
                List<Object> Vendorlist = _vendorService.Bind(id);

                var jsonResult = JsonConvert.SerializeObject(Vendorlist);

                if (jsonResult != null)
                {
                    //return OK(jsonResult);
                }
                return Vendorlist;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }
        [HttpGet("GetUserPermissions")]
        public DataSet GetUserPermissions()
        {

            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@User_Id",User.Identity.Name),//0
                 //new SqlParameter("@term",Url),//0
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectPagePermision", Params);

            return ds;
        }
        [HttpGet("DashBoard")]
        public DataSet DashBoard(string vndr_Id,string Prop_Id,int Days)
        {
            if (vndr_Id == null)
                vndr_Id = "";
            if (Prop_Id == null)
                Prop_Id = "";

            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@vndr_Id",vndr_Id),//0
                     new SqlParameter("@prop_Id",""),//0
                        new SqlParameter("@days",90),//0
                 //new SqlParameter("@term",Url),//0
			};

            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "[proc_dashboard]", Params);

            return ds;
        }
        [HttpGet("allproperty")]
        public HttpResponseMessage GetPropNames(int VendorId)
        {
           
            var list = _vendorService.BindPropertyByVendorId(VendorId);
            var badResponse = Request.CreateResponse(HttpStatusCode.OK, list);
            return badResponse;
            }
        

        [HttpGet("Edit")]
        public List<Object> Edit(int Id)
        {
            try
            {
                List<Object> VendorlistbyId = _vendorService.Edit(Id);
                return VendorlistbyId;
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }

        }
        [HttpDelete("Editd")]

        public HttpResponseMessage Delete(VendorViewModel vendorViewModel)
        {


            TransactionStatus transactionStatus;
            var results = new VendorValidation().Validate(vendorViewModel);

            if (!results.IsValid)
            {
                vendorViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                vendorViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                vendorViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, vendorViewModel);
                return badResponse;
            }
            try
            {
                var vendorBo = BuiltVendorBo1(vendorViewModel);
                transactionStatus = _vendorService.DeleteVendor(vendorBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(vendorViewModel));
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

        [HttpPost("EditVendor")]

        public HttpResponseMessage EditVendor(VendorViewModel vendorViewModel)
        {

            TransactionStatus transactionStatus;
            var results = new VendorValidation().Validate(vendorViewModel);

            if (!results.IsValid)
            {
                vendorViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                vendorViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                vendorViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, vendorViewModel);
                return badResponse;
            }
            try
            {
                var userBo = BuiltVendorBo1(vendorViewModel);
                transactionStatus = _vendorService.EditVendor(userBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(vendorViewModel));
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
        public HttpResponseMessage CreateVendor(VendorViewModel vendorViewModel)
        {

            

            TransactionStatus transactionStatus;
            var results = new VendorValidation().Validate(vendorViewModel);

            if (!results.IsValid)
            {
                vendorViewModel.Errors = GenerateErrorMessage.Built(results.Errors);
                vendorViewModel.ErrorType = ErrorTypeEnum.Error.ToString().ToLower();
                vendorViewModel.Status = false;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, vendorViewModel);
                return badResponse;
            }
            try
            {
                var vendorBo = BuiltVendorBo(vendorViewModel);
                transactionStatus = _vendorService.CreateVendor(vendorBo);

                if (transactionStatus.Status == false)
                {
                    var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(vendorViewModel));
                    return badResponse;
                }
                else
                {
                    transactionStatus.ErrorType = ErrorTypeEnum.Success.ToString();
                    transactionStatus.ReturnMessage.Add("Record successfully inserted to database");

                    var badResponse = Request.CreateResponse(transactionStatus.Id);

                    return badResponse;
                }
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }


        }


        [HttpGet("ClearVendor")]
        public HttpResponseMessage ClearVendor()
        {
            ModelState.Clear();

            var badResponse = Request.CreateResponse();
            return badResponse;
        }

        private VendorBo BuiltVendorBo(VendorViewModel vendorViewModel)
        {
            return (VendorBo)new VendorBo().InjectFrom(vendorViewModel);
        }
        private VendorEditBo BuiltVendorBo1(VendorViewModel vendorViewModel)
        {
            return (VendorEditBo)new VendorEditBo().InjectFrom(vendorViewModel);
        }

        #region RoomRateCalender
        [HttpGet("GetRooms")]
        public HttpResponseMessage GetRooms(int propId)
        {
            try
            {
                var jsonResult = JsonConvert.SerializeObject(_vendorService.GetRoomsRate(propId));
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

        [HttpGet("updateAvailablity")]
        public HttpResponseMessage UpdateAvailablity(int Inv_Id, int Available)
        {
            TransactionStatus transactionStatus;
            try
            {

                transactionStatus = _vendorService.UpdateAvailableRoom(Inv_Id, Available);
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

        [HttpGet("updateRates")]
        public HttpResponseMessage UpdateRates(int Inv_Id, int Price)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _vendorService.UpdateRoomRates(Inv_Id, Price);
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

        [HttpGet("updaterackRates")]
        public HttpResponseMessage updaterackRates(int Inv_Id, int Vndr_Amnt)
        {
            TransactionStatus transactionStatus;
            try
            {
                transactionStatus = _vendorService.updaterackRates(Inv_Id, Vndr_Amnt);
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

        #endregion

        #region FileUpload R&D

        [HttpPost("ImageUpload")]
        public void ImageUpload(List<byte> file)
        {
            try
            {
                //string directoryPath = Server.MapPath(string.Format("~/Areas/Global/Users/" + User.Identity.GetUserId() + "/businessPages"));
                //if (!Directory.Exists(directoryPath))
                //{
                //    Directory.CreateDirectory(directoryPath);
                //    DirectoryManipulator.CreateDirectoryWithPermissions(directoryPath);
                //}
                //directoryPath += "/" + pageId;
                //if (!Directory.Exists(directoryPath))
                //{
                //    Directory.CreateDirectory(directoryPath);
                //    DirectoryManipulator.CreateDirectoryWithPermissions(directoryPath);
                //}
                //directoryPath += "/Logo";
                //if (!Directory.Exists(directoryPath))
                //{
                //    Directory.CreateDirectory(directoryPath);
                //    DirectoryManipulator.CreateDirectoryWithPermissions(directoryPath);
                //}



                using (MemoryStream mem = new MemoryStream(file.ToArray()))
                {
                    var yourImage = Image.FromStream(mem);
                    using (Bitmap b = new Bitmap(yourImage.Width, yourImage.Height))
                    {
                        b.SetResolution(yourImage.HorizontalResolution, yourImage.VerticalResolution);

                        using (Graphics g = Graphics.FromImage(b))
                        {
                            g.Clear(Color.White);
                            g.DrawImageUnscaled(yourImage, 0, 0);
                        }

                        // Now save b as a JPEG like you normally would
                    }

                    //  directoryPath += "/" + pageId + ".jpg";
                    // If you want it as Jpeg
                    //   yourImage.Save(directoryPath.ToString().Trim(), ImageFormat.Jpeg);
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string UploadBlobImage(HttpPostedFileBase image, string containerType)
        {
            try
            {
                if (image != null)
                {
                    if (!string.IsNullOrEmpty(image.FileName))
                    {

                        var blobId = string.Format("{0}", Guid.NewGuid().ToString());
                        var fileStream = image.InputStream;
                        fileStream.Position = 0;

                        App.Common.BlobUtilities.CreateBlob(String.Format("{0}", containerType), blobId, image.ContentType, fileStream);

                        string ImageUrl = App.Common.BlobUtilities.RetrieveBlobUrl(String.Format("{0}", containerType), blobId);
                        return ImageUrl;
                    }
                }
                return null;
            }
            catch (Exception exc)
            {
                ApplicationErrorLogServices.AppException(exc);
                return exc.Message;
            }
        }

        #endregion


    }
}