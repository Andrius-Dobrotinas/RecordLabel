using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using RecordLabel.Catalogue;
using RecordLabel.Web.Controllers;

namespace RecordLabel.Web.ModelBinding
{
    /// <summary>
    /// Selects releases that correspond to posted ReleaseIds from the database and adds them to the model
    /// </summary>
    public class ArtistBinder : BaseWithImagesBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object model = base.BindModel(controllerContext, bindingContext);

            ReleaseContext dbContext = ((IHasDbContext)controllerContext.Controller).DbContext;
            NameValueCollection form = controllerContext.HttpContext.Request.Form;
            if (form.AllKeys.Contains("ReleaseIds"))
            {
                int[] ids = form.GetValues("ReleaseIds").Select(item => int.Parse(item)).ToArray();
                ((Artist)model).Releases = dbContext.Releases.Where(item => ids.Contains(item.Id)).ToArray();
            }

            return model;
        }
    }
}