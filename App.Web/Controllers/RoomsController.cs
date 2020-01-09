using System;
using System.Web.Mvc;
using App.Web.ViewModels;
using App.Web;
using App.UIServices;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using App.DataAccess;
using System.Data.SqlClient;
using System.Data;


namespace App.Web.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        // GET: Rooms
        public ActionResult Create()
        {
            var roomViewModel = new RoomViewModel();

            return View(roomViewModel);
        }

        public ActionResult Bind()
        {

            return View();
        }

        public ActionResult Edit(int Id)
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

        public ActionResult Delete(int Id)
        {

            return View();
        }




        [HttpPost]

        public ActionResult Upload(HttpPostedFileBase photo)
        {

            string imagePath = UploadBlobImage(photo, "Room");

            var cookie = new HttpCookie("Image");
            cookie.Value = imagePath;
            Response.Cookies.Add(cookie);
            return RedirectToAction("Create");
        }

        public ActionResult UploadEdit(HttpPostedFileBase photo)
        {
            string imagePath = UploadBlobImage(photo, "Room");
            var cookie = new HttpCookie("ImageRoom");

            cookie.Value = imagePath;
            Response.Cookies.Add(cookie);

            var Id = Request.Cookies["EditId"].Value;
            return RedirectToAction("Edit/" + Id + "");
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

            return Redirect("/Rooms/bind");
        }

        public ActionResult PropertyRooms()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public string ImageUpload(List<byte> file)
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
    }
}