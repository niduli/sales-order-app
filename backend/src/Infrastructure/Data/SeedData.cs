using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                    new Customer { Name = "ABC Traders", Address1 = "No.10 Main Street", City = "Colombo", Phone = "0771234567" },
                    new Customer { Name = "Sunshine Stores", Address1 = "45 Palm Road", City = "Kandy", Phone = "0779876543" }
                );
            }

            if (!context.Items.Any())
            {
                context.Items.AddRange(
                    new Item { ItemCode = "ITM001", Description = "Laptop", Price = 150000 },
                    new Item { ItemCode = "ITM002", Description = "Mouse", Price = 2500 },
                    new Item { ItemCode = "ITM003", Description = "Keyboard", Price = 5000 }
                );
            }

            context.SaveChanges();
        }
    }
}
