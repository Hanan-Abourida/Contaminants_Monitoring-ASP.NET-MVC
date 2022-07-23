using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Contaminants_Monitoring.Startup))]
namespace Contaminants_Monitoring
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
