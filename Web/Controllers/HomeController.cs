using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RecordLabel.Catalogue;

namespace RecordLabel.Web.Controllers
{
    public sealed class HomeController : BaseController
    {
        public HomeController() : base(new ReleaseContext())
        {
            IndexViewName = "Index";
        }

        public ActionResult ChangeLanguage(string lang)
        {
            Global.SetCurrentLanguage(lang);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [ProcessTempDataError] //TODO: Do I need this attribute here?
        public override ActionResult Index()
        {
            return View(IndexViewName, SelectData());
        }

        private Tuple<Release[], Article[]> SelectData()
        {
            DbContext.MediaTypes.Load();
            Release[] releases = DbContext.Releases.OrderBy(item => item.Id).Take(3).Include(entity => entity.Images).Include(entity => entity.Artist).ToArray();
            Article[] news = DbContext.Articles.OrderBy(item => item.Date).Take(3).Include(entity => entity.Titles).ToArray();
            return new Tuple<Release[], Article[]>(releases, news);
        }

        [ChildActionOnly]
        public ActionResult Error(Exception e)
        {
            return View("~/Views/Shared/Error.cshtml", e);
        }
    }
}