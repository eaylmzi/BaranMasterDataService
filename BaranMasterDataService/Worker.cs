using BaranMasterDataService.Database;
using BaranMasterDataService.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BaranMasterDataService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly BaranMasterDataDBPath _baranMasterDataDBPath;
        private readonly Cron _cronExpression;
        private readonly ServerPath _serverPath;
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        

        private HttpClient client;
        public Worker(ILogger<Worker> logger,BaranMasterDataDBPath baranMasterDataDBPath, Cron cronExpression,ServerPath serverPath)
        {
            _logger = logger;
            _baranMasterDataDBPath = baranMasterDataDBPath;
            _cronExpression = cronExpression;
            _serverPath = serverPath;
            _schedule = CrontabSchedule.Parse(cronExpression.Expression, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }


        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("Shutting down...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;

                if (now > _nextRun)
                {
                    
                    List<CNMaterials> cNMaterialsObjectList = new List<CNMaterials>();
                    DatabaseCommands databaseCommands = new DatabaseCommands(_baranMasterDataDBPath);
                    ServerCommands serverCommands = new ServerCommands(_serverPath, _baranMasterDataDBPath);


                    cNMaterialsObjectList = databaseCommands.findNullFSRDate();

                    if (cNMaterialsObjectList.Count!=0)
                    {
                        serverCommands.postToServer(cNMaterialsObjectList);
                    }

                     
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                   
                }

                await Task.Delay(3000, stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested);
        }
    }
}
