using System;
using System.Web.Mvc;
using App.Web.ViewModels;
using App.Web;
using App.UIServices;
using System.Web;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace App.Web.Controllers
{
   
    public class FacilityController : Controller
    {
        public ActionResult Create()
        {
            var facilityViewModel = new FacilityViewModel();
            return View(facilityViewModel);
        }
        public ActionResult Bind()
        {

            return View();
        }
        public ActionResult Edit(int Id)
        {
            return View();
        }

        public ActionResult Upload(HttpPostedFileBase photo)
        {
            string blobPath = string.Empty;
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/img/Facil_Image/");
            string myguid = Guid.NewGuid().ToString();
            if (photo != null)
                blobPath = UploadBlobImage(photo, "Facility");

            var cookie = new HttpCookie("ImageFacility");
            cookie.Value = blobPath;
            Response.Cookies.Add(cookie);


            return RedirectToAction("Create");
        }


        public ActionResult UploadEdit(HttpPostedFileBase photo, string FacilityId)
        {

            string imagePath = UploadBlobImage(photo, "Facility");

            var cookie = new HttpCookie("ImageFacility");

            cookie.Value = imagePath;
            Response.Cookies.Add(cookie);

            var Id = FacilityId;
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

        [HttpPost]
        [AllowAnonymous]
        public string ImageUpload(List<byte> file)
        {
            string ImageUrl = string.Empty;
            try
            {
                using (System.IO.MemoryStream mem = new System.IO.MemoryStream(file.ToArray()))
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
                    var blobId = string.Format("{0}_{1}", "Facility", Guid.NewGuid().ToString());
                    App.Common.BlobUtilities.CreateBlob("Facility", blobId, "jpeg", mem);
                    ImageUrl = App.Common.BlobUtilities.RetrieveBlobUrl("Facility", blobId);
                }
                return ImageUrl;
            }
            catch (Exception ex)
            {
                ApplicationErrorLogServices.AppException(ex);
                return ImageUrl;
            }
        }
    }
}