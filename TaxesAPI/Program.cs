using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaxesAPI.Models;

namespace TaxesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var db = new TaxesContext();
                    db.TaxesItem.Add(new TaxesItem
                    {
                        Id = 1,
                        Municipality = "istanbul",
                        Date = DateTime.UtcNow,
                        TaxesSchedule = "yearly",
                        TaxesRatio = 4
                    });
                    db.TaxesItem.Add(new TaxesItem {
                        Id = 2,
                        Municipality = "berlin",
                        Date = DateTime.UtcNow.AddDays(-60),
                        TaxesSchedule = "daily",
                        TaxesRatio = 2
                    });
                    db.SaveChanges();
                    db.Dispose();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
