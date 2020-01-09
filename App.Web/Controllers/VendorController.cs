using System;
using System.Web.Mvc;
using App.Web.ViewModels;
using App.Web;
using App.UIServices;
using System.Web;
using System.IO;
using App.UIServices.InterfaceServices;
using System.Collections.Generic;
using App.DataAccess;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using Omu.ValueInjecter;
using System.Net.Http;
using App.BusinessObject;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateInput(false)]
    public class VendorController : Controller
    {
        static int PropId;
        static string imagePath = string.Empty;
        // GET: Vendor
        readonly ILoginServices _loginService;
        readonly IVendorService _vedorservices;

        public VendorController(IVendorService vedorservices, ILoginServices loginService)
        {
            _vedorservices = vedorservices;
            _loginService = loginService;
        }
        #region Vendor Propfile


        public ActionResult FileDemo()
        {
            return View();
        }

        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult Demo2()
        {
            return View();
        }

        [HttpPost]
        public void VendorImageUpload(List<byte> file)
        {
            string directoryPath = Server.MapPath("~/img/Prop_Image/Image_Gallery");
            try
            {

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
                    string format = ".jpg";
                    Stream fileStream = new FileStream((directoryPath + "/" + "Temp" + format).Trim(), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);

                    mem.Position = 0;
                    mem.CopyTo(fileStream);

                    fileStream.Close();


                    using (Stream fileStream2 = System.IO.File.OpenRead(directoryPath + "/" + "Temp.jpg"))
                    {
                        var blobId = string.Format("{0}", Guid.NewGuid().ToString());
                        App.Common.BlobUtilities.CreateBlob("Test", blobId, "jpeg", fileStream2);
                        string ImageUrl = App.Common.BlobUtilities.RetrieveBlobUrl("Test", blobId);
                        // blockBlob.UploadFromStream(fileStream);
                    }
                    // fileStream is not populated


                    //  var fileStream =  yourImage;
                    //  fileStream.Position = 0;


                    // App.Common.BlobUtilities.CreateBlob(String.Format("{0}","BlobTest"), blobId,yourImage., fileStream);


                    // return ImageUrl;
                    //   directoryPath +="abc"+".jpg";
                    // If you want it as Jpeg
                    //  yourImage.Save(directoryPath.ToString().Trim(), ImageFormat.Jpeg);
                }


            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                try
                {
                    string[] picList = Directory.GetFiles(directoryPath, "*.jpg");
                    foreach (string f in picList)
                    {
                        System.IO.File.Delete(f);
                    }
                }
                catch (IOException err)
                {
                    ApplicationErrorLogServices.AppException(err);
                }
            }
        }


        public ActionResult Create()
        {
            try
            {
                var vendorViewModel = new VendorViewModel();

                var cookieId = new HttpCookie("userprofileId");
                cookieId.Value = User.Identity.Name;
                Response.Cookies.Add(cookieId);

                return View(vendorViewModel);
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }




        public string GetLoginVendorId()
        {
            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@UserId",Uid),//0
                 
			};
            string VendorId = "ss";

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectVendorId", Params);
            List<object> lstcityloc = new List<object>();
            while (reader.Read())
            {
                VendorId = reader["User_Type"].ToString();
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return VendorId;
        }

        public int GetPagePermission()
        {
            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@UserId",Uid),//0
                new SqlParameter("@page","Create new Properties")
                 
			};
            int result = 0;

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectpagePermission", Params);
            List<object> lstcityloc = new List<object>();
            while (reader.Read())
            {

                int.TryParse(reader["result"].ToString(), out result);
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return result;
        }

        public JsonResult GetLoginAuthId()
        {
            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@UserId",Uid),//0
			};
            string AuthAndVendorId = "";

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "proc_SelectVendorId", Params);
            List<object> lstcityloc = new List<object>();
            while (reader.Read())
            {
                AuthAndVendorId = reader["Authority_Id"].ToString();
                AuthAndVendorId = AuthAndVendorId + "," + reader["User_Id"].ToString();
                AuthAndVendorId = AuthAndVendorId + "," + reader["User_Type"].ToString();
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return Json(AuthAndVendorId,JsonRequestBehavior.AllowGet);
        }
        public string GetLoginUserId()
        {

            string Uid = User.Identity.Name;

            return Uid;
        }
        public string GetNoOfProperties()
        {
            CemexDb con = new CemexDb();
            string Uid = User.Identity.Name;
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@VendorID",GetLoginVendorId()),//0
                 
			};
            string PropCount = "";

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "[proc_NO_OF_Properties]", Params);
            List<object> lstcityloc = new List<object>();
            while (reader.Read())
            {
                PropCount = reader["avail"].ToString();
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return PropCount;
        }
       
        public ActionResult BindDetails(int id)
        {


            var cookieId = new HttpCookie("userprofileId");
            cookieId.Value = User.Identity.Name;
            Response.Cookies.Add(cookieId);

            var cookieVendId = new HttpCookie("VendId");
            cookieVendId.Value = id.ToString();
            Response.Cookies.Add(cookieVendId);

            return RedirectToAction("Bind", "Vendor");
        }

        public ActionResult Bind()
        {
            try
            {
                _vedorservices.ExecuteAddRoomTimer();  // start the Room inventory Timer
            }
            catch (Exception exc)
            { ApplicationErrorLogServices.AppException(exc); }
            return View();
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                var cookieId = new HttpCookie("EditId");

                cookieId.Value = Id.ToString();
                Response.Cookies.Add(cookieId);
                var cookieVendId = new HttpCookie("VendId");
                cookieVendId.Value = Id.ToString();
                Response.Cookies.Add(cookieVendId);

                return View();
            }

            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        public ActionResult Delete(int Id)
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(VendorViewModel vendorViewModel)
        {
            ModelState.Clear();

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase photo)
        {

            try
            {
                //string path = System.Web.Hosting.HostingEnvironment.MapPath("~/img/Vendor/");
                //string myguid = Guid.NewGuid().ToString();
                //if (photo != null)
                //   photo.SaveAs(path + myguid + photo.FileName);

                string imagePath = UploadBlobImage(photo, "Vendor");

                var cookie = new HttpCookie("ProfileImage");
                cookie.Value = imagePath;
                Response.Cookies.Add(cookie);
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
            }
        }

        public ActionResult UploadEdit(HttpPostedFileBase photo)
        {
            try
            {
                string imagePath = UploadBlobImage(photo, "Vendor");
                var cookie = new HttpCookie("EditVendorImage");

                cookie.Value = imagePath;
                Response.Cookies.Add(cookie);

                var Id = Request.Cookies["EditId"].Value;
                return RedirectToAction("Edit/" + Id + "");
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return null;
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
                        string ImageName = System.IO.Path.GetFileName(image.FileName);

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

        public ActionResult PropertyRateCalender()
        {
            return View();
        }

        public ActionResult RoomRateCalender()
        {
            return View();
        }
        #endregion
        #region Login
        [AllowAnonymous]
        public ActionResult VendorForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("DashBoard", "Vendor");
                else
                    return RedirectToAction("MultipleLoginError", "Error");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewmodel, string returnUrl)
        {

            //if (User.Identity.IsAuthenticated)
            //{

            if (!ModelState.IsValid)
            {
                return View(loginViewmodel);
            }
            try
            {
                var login = BuiltLoginBo(loginViewmodel);
                List<string> lst = _loginService.AuthenticateUser(login);
                string user_Id = string.Empty, Authority_Id = string.Empty, user_Code = string.Empty, user_type = string.Empty;
                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i = i + 3)
                    {
                        user_Id = lst[i];
                        Authority_Id = lst[i + 1];
                        user_type = lst[i + 2];
                    }


                    switch (Authority_Id)
                    {
                        //case "2":

                        //    System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);

                        //    return RedirectToAction("Edit/" + Authority_Id + "", "SystemProfile");

                        //case "1":
                        //    System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                        //    if (user_type == "0")
                        //        return RedirectToAction("Add/" + Authority_Id + "", "SystemProfile");
                        //    else
                        //        return RedirectToAction("Edit/" + Authority_Id + "", "SystemProfile");

                        case "3":
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            if (user_type == "0")
                                return RedirectToAction("Create", "Vendor", null);
                            else
                                //return RedirectToAction("BindDetails", "Vendor", new { id = user_type });
                                return RedirectToAction("DashBoard", "Vendor", new { id = user_type });
                        case "4":
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            if (user_type == "0")
                                return RedirectToAction("UserProfile", "Vendor", null);
                            else

                                return RedirectToAction("PropertyEdit/" + user_type + "", "Vendor", null);
                        case "5":
                            System.Web.Security.FormsAuthentication.SetAuthCookie(user_Id, false);
                            if (user_type == "0")
                                return RedirectToAction("UserProfile", "Vendor", null);
                            else
                                return RedirectToAction("RoomPage/" + user_type + "", "Vendor", null);

                        default:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(loginViewmodel);
                    }
                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(loginViewmodel);
                }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return RedirectToAction("Index");
            }

            //}
            //else
            //{
            //    return RedirectToAction("Index");
            //}

        }

        [AllowAnonymous]
        public ActionResult Logoff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Vendor");
        }


        private LoginBo BuiltLoginBo(LoginViewModel loginVm)
        {
            return (LoginBo)new LoginBo().InjectFrom(loginVm);
        }

        [AllowAnonymous]
        public ActionResult CreatePassword(int Id)
        {
            return View();
        }

        #endregion

        #region Property Pages
        public ActionResult PropertyPage()
        {
            return View();
        }

        // GET: Property
        public ActionResult ProprtyCreate()
        {
            var cookie = new HttpCookie("pId1");

            cookie.Value = "0";
            Response.Cookies.Add(cookie);

            var cookieimg = new HttpCookie("PropertyImageDefault1");
            cookieimg.Value = imagePath;
            Response.Cookies.Add(cookieimg);


            return View();
        }
        public ActionResult RedirectView()
        {

            return View("Create");
        }
        public ActionResult PropertyEdit(int Id)
        {
            var cookieId = new HttpCookie("EditIdforProperty");

            cookieId.Value = Id.ToString();
            Response.Cookies.Add(cookieId);

            return View();
        }
        public ActionResult ImgUpload(HttpPostedFileBase photo)
        {

            imagePath = UploadBlobImage1(photo, "Property");


            return RedirectToAction("Create");
        }
        public ActionResult ImgUploadEdit(HttpPostedFileBase photo)
        {

            string imagePath = UploadBlobImage1(photo, "Property");

            var cookie = new HttpCookie("ImagePropertyd");

            cookie.Value = imagePath;
            Response.Cookies.Add(cookie);

            var Id = Request.Cookies["EditIdforProperty"].Value;
            return RedirectToAction("Edit/" + Id + "");
        }

        public ActionResult UploadMultiple(HttpPostedFileBase photo, FormCollection form)
        {
            CemexDb con = new CemexDb();

            var Image_Name = form["Image_Name"];
            var Image_Descr = form["Image_Descr"];
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/img/Prop_Image/Image_Gallery/");
            string myguid = Guid.NewGuid().ToString();
            if (photo != null)
                photo.SaveAs(path + myguid + photo.FileName);
            string fullpath = "/img/Prop_Image/Image_Gallery/" + myguid + photo.FileName;
            var Prop_Id = Request.Cookies.Get("pId1");

            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                   new SqlParameter("@Prop_Id", Prop_Id.Value),//0
                new SqlParameter("@Image_Name", Image_Name),//0
                 new SqlParameter("@Image_Descr", Image_Descr),//1
                  new SqlParameter("@Image_dir", fullpath),//2
                    new SqlParameter("@Image_CreatedBy", "Samtest"),//3
                      new SqlParameter("@Image_Verified_By", "Samtest"),//3
                     
             
			};

            if (!String.IsNullOrEmpty(Image_Name))
            {
                Params[2].Value = Image_Name;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(Image_Descr))
            {
                Params[3].Value = Image_Descr;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }
            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPropertyImage_Gallery", Params);
            // return ds.Tables[1].Rows[0][0].ToString();
            var cookie = new HttpCookie("pId1");

            cookie.Value = Prop_Id.Value;
            Response.Cookies.Add(cookie);

            return RedirectToAction("RedirectView");
        }

        [HttpPost]
        public ActionResult UploadMultiple1(HttpPostedFileBase photo, FormCollection form)
        {
            CemexDb con = new CemexDb();

            var Image_Name = form["Image_Name"];
            var Image_Descr = form["Image_Descr"];
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/img/Prop_Image/Image_Gallery/");
            string myguid = Guid.NewGuid().ToString();
            if (photo != null)
                photo.SaveAs(path + myguid + photo.FileName);
            string fullpath = "/img/Prop_Image/Image_Gallery/" + myguid + photo.FileName;
            var Prop_Id = Request.Cookies.Get("pId1");

            SqlParameter[] Params = 
			{ 
                   new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                   new SqlParameter("@Prop_Id", Prop_Id.Value),//0
                new SqlParameter("@Image_Name", Image_Name),//0
                 new SqlParameter("@Image_Descr", Image_Descr),//1
                  new SqlParameter("@Image_dir", fullpath),//2
                    new SqlParameter("@Image_CreatedBy", "Samtest"),//3
                      new SqlParameter("@Image_Verified_By", "Samtest"),//3
                     
             
			};

            if (!String.IsNullOrEmpty(Image_Name))
            {
                Params[2].Value = Image_Name;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }
            if (!String.IsNullOrEmpty(Image_Descr))
            {
                Params[3].Value = Image_Descr;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }
            Params[0].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPropertyImage_Gallery", Params);
            // return ds.Tables[1].Rows[0][0].ToString();
            var cookie = new HttpCookie("pId1");

            cookie.Value = Prop_Id.Value;
            Response.Cookies.Add(cookie);

            return RedirectToAction("RedirectView");
        }

        [HttpPost]
        public ActionResult UploadPropertyImages(HttpPostedFileBase photo, PropertyImageViewModel form, string PropertyId)
        {

            //if (!ModelState.IsValid)
            //{
            //    return View(form);
            //}
            CemexDb con = new CemexDb();

            var Image_Name = form.Image_Name;
            var Image_Descr = form.Image_descr;
            try
            {
                string imagePath = UploadBlobImage(photo, "Property");
                var Prop_Id = PropertyId;

                SqlParameter[] Params = 
			    { 
                   new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                   new SqlParameter("@Prop_Id", Convert.ToInt32(PropertyId)),//0
                   new SqlParameter("@Image_Name", "unknown"),//0
                   new SqlParameter("@Image_Descr", "unknown"),//1
                   new SqlParameter("@Image_dir", imagePath),//2
                   new SqlParameter("@Image_CreatedBy", "unknown"),//3
                   new SqlParameter("@Image_Verified_By", "unknown"),//3                
             
			    };


                Params[0].Direction = ParameterDirection.Output;
                DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPropertyImage_Gallery", Params);
                // return ds.Tables[1].Rows[0][0].ToString();
                var cookie = new HttpCookie("pId");

                cookie.Value = PropertyId;
                Response.Cookies.Add(cookie);
                PropId = Convert.ToInt32(PropertyId);
                return RedirectToAction("Bind");
            }
            catch (Exception exc) { return RedirectToAction("Bind"); }
        }

        [HttpPost]
        [AllowAnonymous]
        public void PropertyImageUpload(List<byte> file, int PropertyId)
        {
            // string directoryPath = Server.MapPath(("@img\\Prop_Image\\Image_Gallery"));
            //          string directoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/img/Prop_Image/Image_Gallery");

            string ImageUrl = string.Empty;

            //if (!Directory.Exists(directoryPath))
            //{
            //    Directory.CreateDirectory(directoryPath);
            //    DirectoryManipulator.CreateDirectoryWithPermissions(directoryPath);
            //}
            try
            {
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
                    }

                    mem.Position = 0;
                    var blobId = string.Format("{0}-{1}_{2}", "Property", PropertyId.ToString(), Guid.NewGuid().ToString());
                    App.Common.BlobUtilities.CreateBlob("Property", blobId, "jpeg", mem);
                    ImageUrl = App.Common.BlobUtilities.RetrieveBlobUrl("Property", blobId);

                }

                try
                {
                    CemexDb con = new CemexDb();
                    SqlParameter[] Params = 
			         { 
                             new SqlParameter("@opReturnValue", SqlDbType.Int),//0
                             new SqlParameter("@Prop_Id", PropertyId),//0
                             new SqlParameter("@Image_Name", "unknown"),//0
                             new SqlParameter("@Image_Descr", "unknown"),//1
                             new SqlParameter("@Image_dir", ImageUrl),//2
                             new SqlParameter("@Image_CreatedBy", "unknown"),//3
                             new SqlParameter("@Image_Verified_By", "unknown"),//3                
  			         };
                    Params[0].Direction = ParameterDirection.Output;
                    DataSet ds = SqlHelper.ExecuteDataset(con.GetConnection(), CommandType.StoredProcedure, "proc_AddPropertyImage_Gallery", Params);
                }
                catch (Exception exc) { ApplicationErrorLogServices.AppException(exc); }
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
            }
            finally
            {

            }
        }

        public string UploadBlobImage1(HttpPostedFileBase image, string containerType)
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

        #region Room Pages
        // GET: Rooms
        public ActionResult RoomCreate()
        {
            var roomViewModel = new RoomViewModel();

            return View(roomViewModel);
        }

        public ActionResult RoomPage()
        {

            return View();
        }

        public ActionResult RoomEdit(int Id)
        {
            var EditId = new HttpCookie("EditId");

            EditId.Value = Id.ToString();
            Response.Cookies.Add(EditId);
            return View();
        }

        public ActionResult Editpolicy(int Id)
        {

            return View();
        }

        public ActionResult RoomDelete(int Id)
        {

            return View();
        }




        [HttpPost]

        public ActionResult RoomUpload(HttpPostedFileBase photo)
        {

            string imagePath = UploadBlobImage2(photo, "Room");

            var cookie = new HttpCookie("Image");
            cookie.Value = imagePath;
            Response.Cookies.Add(cookie);
            return RedirectToAction("Create");
        }

        public ActionResult RoomUploadEdit(HttpPostedFileBase photo)
        {
            string imagePath = UploadBlobImage2(photo, "Room");
            var cookie = new HttpCookie("ImageRoom");

            cookie.Value = imagePath;
            Response.Cookies.Add(cookie);

            var Id = Request.Cookies["EditId"].Value;
            return RedirectToAction("Edit/" + Id + "");
        }

        public string UploadBlobImage2(HttpPostedFileBase image, string containerType)
        {
            try
            {
                if (image != null)
                {
                    if (!string.IsNullOrEmpty(image.FileName))
                    {
                        string ImageName = System.IO.Path.GetFileName(image.FileName);

                        var blobId = string.Format("{0}", image.FileName);
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

        public ActionResult Save(RoomViewModel model)
        {
            string Policy_Name = model.Policy_Name;
            string Policy_Descr = model.Policy_Descr;

            var q = new RoomsApiController(model);

            return Redirect("/Vendor/RoomPage");
        }

        public ActionResult PropertyRooms()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public string RoomImageUpload(List<byte> file)
        {
            string ImageUrl = string.Empty;
            try
            {
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
                    }
                    mem.Position = 0;
                    var blobId = string.Format("{0}_{1}", "Room", Guid.NewGuid().ToString());
                    App.Common.BlobUtilities.CreateBlob("Room", blobId, "jpeg", mem);
                    ImageUrl = App.Common.BlobUtilities.RetrieveBlobUrl("Room", blobId);
                }
                return ImageUrl;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return ImageUrl;
            }
        }
        [HttpGet]
        public string GetAmountType(int Prop_Id)
        {
            CemexDb con = new CemexDb();

            SqlParameter[] Params = 
			{ 
                new SqlParameter("@Id",Prop_Id),//0
			};
            string RateType = "";

            SqlDataReader reader = SqlHelper.ExecuteReader(con.GetConnection(), CommandType.StoredProcedure, "[proc_SelectAmountType]", Params);
            while (reader.Read())
            {
                RateType = reader["Pricing_Type"].ToString();
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return RateType;


        }

        [HttpGet]
        public string GetRoomImageUrl()
        {
            return RoomImage.ImageUrl;
        }

        public string ImageUploadOld(List<byte> file)
        {
            string directoryPath = Server.MapPath("~/img/Prop_Image/Image_Gallery");
            string ImageUrl = string.Empty;
            try
            {
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
                    }
                    string format = ".jpg";
                    using (Stream fileStream = new FileStream((directoryPath + "/" + "Temp" + format).Trim(), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        mem.Position = 0;
                        mem.CopyTo(fileStream);
                    }

                    using (Stream fileStream = System.IO.File.OpenRead(directoryPath + "/" + "Temp.jpg"))
                    {
                        var blobId = string.Format("{0}_{1}", "Room", Guid.NewGuid().ToString());
                        App.Common.BlobUtilities.CreateBlob("Room", blobId, "jpeg", fileStream);
                        ImageUrl = App.Common.BlobUtilities.RetrieveBlobUrl("Room", blobId);
                    }
                }
                return ImageUrl;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return ImageUrl;
            }
            finally
            {
                try
                {
                    string[] picList = System.IO.Directory.GetFiles(directoryPath, "*.jpg");
                    foreach (string f in picList)
                    {
                        System.IO.File.Delete(f);
                    }
                }
                catch (IOException err)
                {
                    ApplicationErrorLogServices.AppException(err);
                }
            }
        }
        #endregion


        #region UserProfile


        // GET: UserProfile
        public ActionResult UserProfile()
        {

            string user_Id = User.Identity.Name;
            return View();
        }
        public ActionResult Parameters()
        {
            return View();
        }
        public ActionResult UserProfileView()
        {

            string user_Id = User.Identity.Name;
            return View();
        }
        public ActionResult UserProfileViewUsers()
        {

            string user_Id = User.Identity.Name;
            return View();
        }

        #endregion

        #region reports

        // GET: Booking
        public ActionResult BookingsReport()
        {
            return View();
        }
        public ActionResult ReportTransactions()
        {
            return View();
        }
        public ActionResult UpcomingBookings()
        {
            return View();
        }
        public ActionResult UpcomingBookingTransactions()
        {
            return View();
        }
        public ActionResult ConsumerReport()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Dashboard()
        {
            return View();
        }

        public void Export()
        {

            string data = Request.Form["data"];
            data = HttpUtility.UrlDecode(data);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=VendorBooking.xls");
            Response.Charset = "";
            Response.ContentType = "application/excel";
            System.Web.HttpContext.Current.Response.Write(data);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();

            //return View();
        }
        #endregion

    }
}