using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CyberFortress.Startup))]
namespace CyberFortress
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
