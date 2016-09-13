using RecordLabel.Content;
using RecordLabel.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RecordLabel.Web
{
    public static class DisplayTemplates
    {
        public static MvcHtmlString ListItemWithImage(this HtmlHelper helper, BaseWithImages model, bool adminMode)
        {
            return ListItemWithImage(helper, model, adminMode, null);
        }

        public static MvcHtmlString ListItemWithImage(this HtmlHelper helper, BaseWithImages model, bool adminMode, string targetControllerName)
        {
            return ListItemWithImage(helper, new ListItemWithImageModel { Model = model, AdminMode = adminMode, TargetControllerName = targetControllerName });
        }

        public static MvcHtmlString ListItemWithImage(this HtmlHelper helper, ListItemWithImageModel model)
        {
            return helper.Partial("ExtensionPartials/ListItemWithImage", model);
        }
    }
}