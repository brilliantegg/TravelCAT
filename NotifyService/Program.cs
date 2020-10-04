using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotifyService.Models;

namespace NotifyService
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
              var optionsBuilder = new DbContextOptionsBuilder<CUSERSAUDREDESKTOPTRAVELCATTRAVELCATAPP_DATATRAVEL_CAT_V1MDFContext>();
              optionsBuilder.UseSqlServer(hostContext.Configuration.GetSection("ConnectionString").Value);
              services.AddScoped<CUSERSAUDREDESKTOPTRAVELCATTRAVELCATAPP_DATATRAVEL_CAT_V1MDFContext>(s => new CUSERSAUDREDESKTOPTRAVELCATTRAVELCATAPP_DATATRAVEL_CAT_V1MDFContext(optionsBuilder.Options));

              services.AddHostedService<Worker>();
            });
  }
}
