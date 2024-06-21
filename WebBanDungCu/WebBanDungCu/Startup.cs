using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBanDungCu.Startup))]
namespace WebBanDungCu
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
