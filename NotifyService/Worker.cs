using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotifyService.Models;

namespace NotifyService
{
  public class Worker : BackgroundService
  {
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    {
      _logger = logger;
      _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
          var context = scope.ServiceProvider.GetRequiredService<CUSERSAUDREDESKTOPTRAVELCATTRAVELCATAPP_DATATRAVEL_CAT_V1MDFContext>();
          // now do your work

          var tmp = context.Activity.First();
          _logger.LogInformation("yooo! {}", tmp.ActivityTitle);
          _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
          await Task.Delay(1000, stoppingToken);
        }
      }
    }
  }
}
