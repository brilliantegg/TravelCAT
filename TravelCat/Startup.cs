using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(TravelCat.Startup))]
namespace TravelCat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
