using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rsetful.Startup))]
namespace Rsetful
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
