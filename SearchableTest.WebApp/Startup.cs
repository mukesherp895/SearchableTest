using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SearchableTest.WebApp.Startup))]
namespace SearchableTest.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
