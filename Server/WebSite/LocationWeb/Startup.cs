using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebLocation.Startup))]
namespace WebLocation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
