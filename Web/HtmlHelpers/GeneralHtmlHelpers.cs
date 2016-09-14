using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using RecordLabel.Content;
using System.Web;

namespace RecordLabel.Web
{
    public static class GeneralHtmlHelpers
    {
        /// <summary>
        /// Builds an element for loading more items in the list
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="totalItemCount">A total number of items of a given type in the database</param>
        /// <param name="filterSourceId">An Id of the model used to filter out items in the list (optional - only if filtered)</param>
        /// <returns></returns>
        public static MvcHtmlString LoadMoreBar(this HtmlHelper helper, int totalItemCount, int? filterSourceId = null)
        {
            if (totalItemCount > Settings.ListItemBatchSize)
            {
                string controller = helper.ViewContext.RequestContext.RouteData.Values["controller"] as string;
                string action = helper.ViewContext.RequestContext.RouteData.Values["action"] as string;
                if (String.IsNullOrEmpty(action) || action.Equals("index", StringComparison.OrdinalIgnoreCase))
                {
                    action = "List"; //redirect to List action
                }

                return helper.Partial("ExtensionPartials/LoadMoreBar", new LoadMoreBarModel(action, controller, totalItemCount, filterSourceId));
            }
            return null;
        }

        /// <summary>
        /// Returns Edit buttons for model
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controllerName"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public static HtmlString EditButton(this HtmlHelper helper, string controllerName, int modelId)
        {
            HtmlString result = helper.ActionLink(" ", "Edit", controllerName, new { Id = modelId }, new { @class = "glyphicon-pencil right-top-btn", data_action = "edit", role = "button" });
            return helper.Partial("ExtensionPartials/EditButtons", result);
        }

        public static HtmlString BackButtonBar(this HtmlHelper helper, string text, string action = "Index")
        {
            ViewDataDictionary viewData = new ViewDataDictionary();
            viewData.Add("text", text);
            viewData.Add("action", action);
            return helper.Partial("ExtensionPartials/SingleButtonBar", viewData);
        }
    }
}