using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HangmanMVC.Web.Startup))]
namespace HangmanMVC.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
