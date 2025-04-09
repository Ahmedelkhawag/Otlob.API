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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status)
              .HasConversion(
              // DB => string "Pending"
              orderStatusApp => orderStatusApp.ToString(),
              // APP => OrderStatus.Pending
              orderStatusDb => (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatusDb)
              );

            builder.Property(order => order.SubTotal).HasColumnType("decimal(10,2)");

            // Aggregation Relationship
            builder.OwnsOne(order => order.ShippingAddress, sa => sa.WithOwner());
        }
    }
}
