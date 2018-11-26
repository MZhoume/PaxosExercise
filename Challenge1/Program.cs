using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Challenge1
{
    /// <summary>
    /// Entry point of the service. It wires up the corresponding web host and start taking requests.
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
