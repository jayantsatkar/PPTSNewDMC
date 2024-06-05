using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;

namespace FactoryMagix
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Logger.Info("Application started");
        }

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            //log4net.ILog log = log4net.LogManager.GetLogger(typeof(MvcApplication));
            Logger.Error(exception);
            Server.ClearError();
            // Response.Redirect("~/Error/GeneralError");
        }
    }
}
