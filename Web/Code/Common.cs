using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RecordLabel.Web
{
    public static class Common
    {
        public static TController CreateController<TController>() where TController : Controller, new()
        { 
            RouteData routeData = new RouteData();
            string controllerName = typeof(TController).Name.Replace("Controller", String.Empty);
            routeData.Values.Add("controller", controllerName);

            HttpContextWrapper httpCtxWrapper = new HttpContextWrapper(HttpContext.Current);
            TController controller = new TController();

            controller.ControllerContext = new ControllerContext(httpCtxWrapper, routeData, controller);
            return controller;
        }
    }
}