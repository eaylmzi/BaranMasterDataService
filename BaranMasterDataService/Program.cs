using BaranMasterDataService.Database;
using BaranMasterDataService.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaranMasterDataService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    BaranMasterDataDBPath baranMasterDataDBPath = configuration.GetSection("BaranMasterDataDBPath").Get<BaranMasterDataDBPath>();
                    Cron expression = configuration.GetSection("TimeSettings").Get<Cron>();
                    ServerPath serverPath = configuration.GetSection("ServerPath").Get<ServerPath>();

                    services.AddSingleton(serverPath);
                    services.AddSingleton(expression);
                    services.AddSingleton(baranMasterDataDBPath);                
                    services.AddHostedService<Worker>();
                });
    }
}
