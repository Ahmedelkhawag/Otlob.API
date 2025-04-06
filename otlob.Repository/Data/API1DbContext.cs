using Microsoft.EntityFrameworkCore;
using Otlob.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Repository.Data
{
    public class API1DbContext : DbContext
    {
        public API1DbContext(DbContextOptions<API1DbContext> options) : base(options)
        {
            // We will store the connection string in AppSettings.json file instead of hard coding here
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // To Read Any Entity's Configurations Class
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
