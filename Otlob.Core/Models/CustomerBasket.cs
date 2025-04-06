using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Models
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }

        // To Get Customer Basket Id
        public CustomerBasket(string id)
        {
            Id = id;
        }
    }
}
