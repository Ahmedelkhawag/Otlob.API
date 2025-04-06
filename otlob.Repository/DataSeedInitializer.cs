using Microsoft.EntityFrameworkCore;
using Otlob.Core.Models;
using Otlob.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Otlob.Repository
{
    public class DataSeedInitializer
    {
        public static async Task SedDataAsync(API1DbContext context)
        {
            #region Product Brands
            if (!await context.ProductBrands.AnyAsync())  // Check If No Data in Database
            {
                // 1-Serialize Data (Read and Covert The Data in File To a String)

                var brandData = File.ReadAllText("../Otlob.Repository/Data/DataSeed/Brands.json");

                // 2-Deserialize Data (Convert The String Data To List Of Specific Object)

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                if (brands.Count() > 0)// Check brands is not null & more than 0
                {
                    // 3-Add Data In Database

                    await context.AddRangeAsync(brands);

                    await context.SaveChangesAsync();
                }
            }
            #endregion
            #region  Product Types
            if (!await context.ProductTypes.AnyAsync())
            {
                var typeData = File.ReadAllText("../Otlob.Repository/Data/DataSeed/Types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                if (Types.Count() > 0)
                {
                    await context.AddRangeAsync(Types);

                    await context.SaveChangesAsync();
                }
            }
            #endregion

            #region  Products
            if (!await context.Products.AnyAsync())
            {
                var productData = File.ReadAllText("../Otlob.Repository/Data/DataSeed/Products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                if (products.Count() > 0)
                {
                    await context.AddRangeAsync(products);

                    await context.SaveChangesAsync();
                }
            }
            #endregion

        }
    }
}
