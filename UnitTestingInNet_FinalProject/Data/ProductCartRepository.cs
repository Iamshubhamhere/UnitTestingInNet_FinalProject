using Microsoft.EntityFrameworkCore;
using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public class ProductCartRepository : IRepository<ProductCart>
    {
        private EcommerceContext _context;

        public ProductCartRepository(EcommerceContext context)
        {
            _context = context;
        }
        public ProductCart Get(Guid Id)
        {
            return _context.productCarts.FirstOrDefault(r => r.Id == Id);
        }

        public ICollection<ProductCart> GetAll()
        {
            return _context.productCarts.Include(pc => pc.Product).ToList();
        }
        public void Add(ProductCart entity)
        {
            _context.productCarts.Add(entity);
            _context.SaveChanges();
        }
        public void Update(ProductCart entity)
        {
            _context.productCarts.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(ProductCart entity)
        {
            if (entity != null)
            {
                _context.productCarts.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Remove(ProductCart entity)
        {
            _context.productCarts.Remove(entity); 
            _context.SaveChanges();
        }
        public ICollection<ProductCart> SearchProducts(string key)
        {
            return null;
        }
    }
}
