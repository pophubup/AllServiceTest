using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haha
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostContext, config) =>
            {
                var env = hostContext.HostingEnvironment;
                if (env.IsDevelopment())
                {
                    config.AddJsonFile(@"C:\Users\Yohoo\Desktop\key.json", optional: true, reloadOnChange: true);
                   
                }
                if (env.IsProduction())
                {
                    config.AddJsonFile(@"D:\home\key.json", optional: true, reloadOnChange: true);
                   
                }
               
            })
             .ConfigureWebHostDefaults(webBuilder =>
              {
                    webBuilder.UseStartup<Startup>();
            });
    }
}
