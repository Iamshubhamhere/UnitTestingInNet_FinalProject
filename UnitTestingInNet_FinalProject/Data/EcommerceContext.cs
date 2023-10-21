using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext>options ):base(options) 
        {
        }

        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Cart> Carts { get; set; } = default!;
        public DbSet<Catalogue> Catalogues { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<Country> Countries { get; set; } = default!;
        public DbSet<ProductCart> productCarts { get; set; } = default!;
    }
}
