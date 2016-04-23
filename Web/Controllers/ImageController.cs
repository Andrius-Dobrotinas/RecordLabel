using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecordLabel.Catalogue;
using System.Net.Http;

namespace RecordLabel.Web.Controllers
{
    public class ImageController : Controller
    {
        /// <summary>
        /// Returns an image selector/previewer as a partial view
        /// </summary>
        /// <returns></returns>
        public PartialViewResult AddImage()
        {
            return PartialView("~/Views/BaseWithImages/AddImage.cshtml");
        }

        [HttpPost]
        public HttpResponseMessage Delete(int id)
        {
            if (id == 0) throw new IndexOutOfRangeException();

            ReleaseContext db = new ReleaseContext();
            ImageHelper.DeleteImage(db, db.Images.Single(img => img.Id == id));
            db.SaveChanges();

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        /*[System.Web.Http.HttpPost]
        public HttpResponseMessage Image() //(IHttpActionResult)
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[0];

            HttpContext.Current.Response.ContentType = "text/plain";
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new {
                name = file.FileName
            };

            HttpContext.Current.Response.Write(serializer.Serialize(result));
            HttpContext.Current.Response.StatusCode = 200;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }*/
    }
}