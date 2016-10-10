using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using RecordLabel.Web.Controllers;

namespace RecordLabel.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/Web.config")));

            //Remove unnecessary view engines and add a custom C#-only razor view engine
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngineForCs());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BindersConfig.RegisterBinders(ModelBinders.Binders);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Settings.LoadApplicationConfiguration();

#if (true == false)
            //Reinitialize the database with test data
            System.Data.Entity.Database.SetInitializer(new Content.Configurations.DropCreateAndSeedInitializer<RecordLabel.Content.ReleaseContext>());
#endif
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();

            Logger.LogError(exception);

            HomeController controller = Common.CreateController<HomeController>();
            //Users don't have to know any details about this exception - show it only to an admin
            ActionResult result = controller.Error(Global.IsAdminMode ? exception : new Exception(Localization.ApplicationLocalization.UnexpectedError));
            result.ExecuteResult(controller.ControllerContext);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            Global.SetCurrentThreadCultureFromSession();
        }
    }
}
