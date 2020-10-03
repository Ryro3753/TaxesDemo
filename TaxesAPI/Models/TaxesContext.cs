using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxesAPI.Models
{
    public class TaxesContext : DbContext
    {
        public TaxesContext()
        {
               
        }
        public TaxesContext(DbContextOptions<TaxesContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxesItem>().HasData(
            new TaxesItem
            {
                Id = 1,
                Municipality = "Istanbul",
                Date = DateTime.UtcNow,
                TaxesSchedule = "yearly",
                TaxesRatio = 4
            },
            new TaxesItem
            {
                Id = 2,
                Municipality = "Berlin",
                Date = DateTime.UtcNow.AddDays(-60),
                TaxesSchedule = "montly",
                TaxesRatio = 2
            }
               );
            base.OnModelCreating(modelBuilder);

        }
        public  DbSet<TaxesItem> TaxesItem { get; set; }

    }
}
