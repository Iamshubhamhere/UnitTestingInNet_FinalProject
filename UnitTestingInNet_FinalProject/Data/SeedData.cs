using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            EcommerceContext context = new EcommerceContext(serviceProvider.GetRequiredService<DbContextOptions<EcommerceContext>>());


            // Ensure the database is properly set up
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            try
            {     // Seeding Products
                if (!context.Products.Any())
                {
                    var products = new List<Product>
              {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Laptop",
                    Description = "15-inch laptop with SSD storage",
                    Price = 799.99m,
                    AvailableQuantity = 10
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Smartphone",
                    Description = "Latest Android smartphone with high-resolution camera",
                    Price = 499.99m,
                    AvailableQuantity = 15
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Headphones",
                    Description = "Wireless over-ear headphones with noise-cancellation",
                    Price = 199.99m,
                    AvailableQuantity = 25
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Tablet",
                    Description = "10-inch tablet with stylus support",
                    Price = 299.99m,
                    AvailableQuantity = 12
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Gaming Console",
                    Description = "High-performance gaming console with 4K support",
                    Price = 449.99m,
                    AvailableQuantity = 8
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Smartwatch",
                    Description = "Fitness and health-tracking smartwatch",
                    Price = 149.99m,
                    AvailableQuantity = 20
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Camera",
                    Description = "Mirrorless camera with interchangeable lenses",
                    Price = 799.99m,
                    AvailableQuantity = 5
                }
            };

                    context.Products.AddRange(products);
                     // Save the changes to the database

                }

                if (!context.Countries.Any())
                {
                    var countries = new List<Country>
                        {
                            new Country
                            {
                                Name = "Canada",
                                ConversionRate = 1.0m, // Conversion rate for Canada
                                TaxRate = 0.07m // 7% tax rate for Canada
                            },
                            new Country
                            {
                                Name = "United States",
                                ConversionRate = 1.2m, // Conversion rate for the United States
                                TaxRate = 0.08m // 8% tax rate for the United States
                            },
                            new Country
                            {
                                Name = "United Kingdom",
                                ConversionRate = 1.5m, // Conversion rate for the UK
                                TaxRate = 0.1m // 10% tax rate for the UK
                            }
                        };

                    context.Countries.AddRange(countries);
                   
                }
                context.SaveChanges();
                if (context.Carts.Any())
                {
                    Cart cart = new Cart();
                    context.Carts.Add(cart);
                    context.SaveChanges();
                }
            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        }
    }
}