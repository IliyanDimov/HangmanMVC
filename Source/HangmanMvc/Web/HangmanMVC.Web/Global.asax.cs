using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HangmanMVC.Web.App_Start;
using System.Data.Entity;
using HangmanMVC.Data;
using HangmanMVC.Data.Migrations;

namespace HangmanMVC.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            AutofacConfig.RegisterAutofac();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
