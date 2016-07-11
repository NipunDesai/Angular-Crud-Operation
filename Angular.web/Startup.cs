using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Angular.web.Startup))]
namespace Angular.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
