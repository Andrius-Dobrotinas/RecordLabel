using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RecordLabel.Content;

using RecordLabel.Web;
using System.Linq.Expressions;
using System.Web.Mvc.Filters;

namespace RecordLabel.Web.Controllers
{
    /// <summary>
    /// Base controller with database context
    /// </summary>
    public abstract class BaseController : Controller, IHasDbContext
    {
        private ReleaseContext db;
        public ReleaseContext DbContext => db;

        protected string IndexViewName { get;set; }
        protected string EditViewName { get; set; }

        /// <summary>
        /// Doesn't use any database context
        /// </summary>
        public BaseController() : this(null)
        {

        }

        public BaseController(ReleaseContext dbContext)
        {
            db = dbContext;
            IndexViewName = "List";
            EditViewName = "Edit";
        }

        public abstract ActionResult Index();

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            base.OnAuthentication(filterContext);

            //Set Admin mode
            if (HttpContext.User.Identity.IsAuthenticated == true)
            {
                HttpContext.Session.Add("AdminMode", true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}