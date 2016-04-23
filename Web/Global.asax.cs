using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using RecordLabel.Catalogue;
using RecordLabel.Web.Controllers;
using System.Web.Mvc.Html;

namespace RecordLabel.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //Remove unnecessary view engines and add a custom C#-only razor view engine
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngineForCs());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BindersConfig.RegisterBinders(ModelBinders.Binders);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Settings.LoadApplicationConfiguration();

#if (true == false)
            //Reinitialize the database with test data
            Database.SetInitializer(new Catalogue.Configurations.DropCreateAndSeedInitializer<ReleaseContext>());
#endif
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();

            //log error

            HomeController controller = Common.CreateController<HomeController>();
            //Users don't have to know any details about this exception - show it only to an admin
            ActionResult result = controller.Error(Global.IsAdminMode ? exception : new Exception(Localization.UnexpectedError));
            result.ExecuteResult(controller.ControllerContext);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //Set current culture from session
            if (HttpContext.Current.Session != null)
            {
                CultureInfo culture = HttpContext.Current.Session["Culture"] as CultureInfo ?? CultureInfo.GetCultureInfo(LocalStringSet.English);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }
    }
}
