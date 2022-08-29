using CGHangfireApp.Helper;
using Hangfire;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartup(typeof(HangfireStartup))]
namespace CGHangfireApp.Helper
{
    /// <summary>
    /// 
    /// </summary>
    internal class HangfireStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfireDashboard("/dashboard");                 
        }
    }
}
