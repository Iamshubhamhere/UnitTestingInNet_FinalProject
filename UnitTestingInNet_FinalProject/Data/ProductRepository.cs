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
            return _context.Products.OrderBy(product => product.Name).ToList();
        }

        public void Add(Product entity)
        {
            _context.Products.Add(entity);
            _context.SaveChanges();
        }
        public void Update(Product entity)
        {
            _context.Products.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(Product entity)
        {
            if (entity != null)
            {
                _context.Products.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Remove(Product entity)
        {
            _context.Products.Remove(entity);
            _context.SaveChanges();
        }

        public ICollection<Product> SearchProducts(string key)
        {
            ICollection<Product>search = _context.Products.Where(pr =>pr.Name.Contains(key) || pr.Description.Contains(key)).OrderBy(pr =>pr.Name).ToList();
            return search;
        }
        /* void IRepository<Product>.Add(Product entity)
         {
             // This method is explicitly implemented and is not accessible outside of the ProductRepository.
         }*/
    }
}
