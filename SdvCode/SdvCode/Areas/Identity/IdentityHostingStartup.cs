using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SdvCode.Areas.Identity.IdentityHostingStartup))]

namespace SdvCode.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}