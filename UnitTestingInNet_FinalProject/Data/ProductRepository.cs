using Microsoft.EntityFrameworkCore;
using UnitTestingInNet_FinalProject.Models;
namespace UnitTestingInNet_FinalProject.Data
{
    public class ProductRepository : IRepository<Product> 
    {
        private EcommerceContext _context;
       public ProductRepository( EcommerceContext context) { 
        _context = context; 
        
        }

        public Product Get(Guid Id)
        {
            return _context.Products.FirstOrDefault(r => r.Id == Id);
        }

        public ICollection<Product> GetAll()
        {
            return _context.Products.ToList();
        }
       public Product AddItemToCart(Guid id, Product product)
        {
            return product;
        }
    }
}
