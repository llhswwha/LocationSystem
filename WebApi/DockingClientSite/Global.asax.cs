using BLL;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebNSQLib;

namespace Rsetful
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bll bll = new Bll();

            RealAlarm ra = new RealAlarm();
            Thread th = new Thread(ra.ReceiveRealAlarmInfo);
            th.Start();

        }
    }
}
