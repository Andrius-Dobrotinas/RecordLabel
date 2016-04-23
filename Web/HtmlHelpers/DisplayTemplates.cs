using RecordLabel.Catalogue;
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
            return helper.Partial("ExtensionPartials/ListItemWithImage", new Tuple<BaseWithImages, bool>(model, adminMode));
        }
    }
}