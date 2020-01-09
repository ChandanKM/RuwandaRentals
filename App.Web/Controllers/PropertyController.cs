using App.DataAccess;
using App.UIServices;
using App.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    [Authorize]
    public class PropertyController : Controller
    {
        // GET: Property
        static int PropId;
        static string imagePath = string.Empty;
        public ActionResult Bind()
        {
            return View();
        }

        // GET: Property
        public ActionResult Create()
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
        public ActionResult Edit(int Id)
        {
            var cookieId = new HttpCookie("EditIdforProperty");

            cookieId.Value = Id.ToString();
            Response.Cookies.Add(cookieId);

            return View();
        }
        public ActionResult Upload(HttpPostedFileBase photo)
        {

            imagePath = UploadBlobImage(photo, "Property");


            return RedirectToAction("Create");
        }
        public ActionResult UploadEdit(HttpPostedFileBase photo)
        {

            string imagePath = UploadBlobImage(photo, "Property");

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
        public void ImageUpload(List<byte> file, int PropertyId)
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
    }
}