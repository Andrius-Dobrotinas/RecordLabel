using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecordLabel.Web
{
    /// <summary>
    /// Checks TempData for "Error" key and, if found, adds its value to ModelState Errors
    /// </summary>
    public class ProcessTempDataError : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.TempData.ContainsKey("Error"))
            {
                filterContext.Controller.ViewData.ModelState.AddModelError(String.Empty, filterContext.Controller.TempData["Error"].ToString());
            }
            base.OnActionExecuting(filterContext);
        }
    }
}