using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxesAPI.Models
{
    public class TaxesContext : DbContext
    {
        public TaxesContext(DbContextOptions<TaxesContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public  DbSet<TaxesItem> TaxesItem { get; set; }

    }
}
