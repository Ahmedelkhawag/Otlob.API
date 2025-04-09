using Otlob.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Order_Aggregate
{
    public class OrderItem :BaseEntity
    {
        public ProductItemOrdered Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public OrderItem()
        {
        }
        public OrderItem(ProductItemOrdered product, int quantity, decimal price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }

        
    }
}
