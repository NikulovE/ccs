using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using WebAPI2.Models;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(WebAPI2.Startup))]

namespace WebAPI2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // настраиваем контекст и менеджер
            app.MapSignalR();
            //app.CreatePerOwinContext<ApplicationContext>(ApplicationContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}
