using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E7Store.Startup))]
namespace E7Store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
