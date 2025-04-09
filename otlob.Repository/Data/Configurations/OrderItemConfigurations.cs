using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otlob.Core.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Repository.Data.Configurations
{
    public class OrderItemConfigurations:IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(item => item.Price)
             .HasColumnType("decimal(10,2)");

            builder.OwnsOne(item => item.Product, p => p.WithOwner());
        }
    }
    
}
