using Microsoft.AspNet.SignalR;
using Owin;

namespace iConnect.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfig = new HubConfiguration {EnableDetailedErrors = true};
            //hubConfig.Resolver = 
            app.MapSignalR(hubConfig);
        }
    }
}