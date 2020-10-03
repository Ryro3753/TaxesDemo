using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaxesAPI.Models;
using TaxesAPI.Services;

namespace TaxesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<TaxesContext>(opt =>
                opt.UseInMemoryDatabase("Taxes")); //At production we have to use a persistence database like PostgreSQL, Oracle, MSSQL.

            services.AddTransient<ITaxesService, TaxesService>();
            services.AddAutoMapper(i => i.AddProfile<DTOProfile>());

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
              var db = new TaxesContext();
                    db.TaxesItem.Add(new TaxesItem
                    {
                        Id = 1,
                        Municipality = "Istanbul",
                        Date = DateTime.UtcNow,
                        TaxesSchedule = "yearly",
                        TaxesRatio = 4
                    });
                    db.TaxesItem.Add(new TaxesItem {
                        Id = 2,
                        Municipality = "Berlin",
                        Date = DateTime.UtcNow.AddDays(-60),
                        TaxesSchedule = "montly",
                        TaxesRatio = 2
                    });
                    db.SaveChanges();
                    db.Dispose();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
