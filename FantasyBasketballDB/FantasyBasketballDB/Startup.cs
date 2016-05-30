using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FantasyBasketballDB.Startup))]
namespace FantasyBasketballDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
