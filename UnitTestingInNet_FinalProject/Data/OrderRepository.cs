using Microsoft.EntityFrameworkCore;
using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public class OrderRepository : IRepository<Order>
    {
        private EcommerceContext _context;

        public OrderRepository(EcommerceContext context)
        {
            _context = context;
        }
        public Order Get(Guid Id)
        {
            return _context.Orders.FirstOrDefault(r => r.Id == Id);
        }

        public ICollection<Order> GetAll()
        {
            return _context.Orders.ToList();
        }
        public void Add(Order entity)
        {
            _context.Orders.Add(entity);
            _context.SaveChanges();
        }
        public void Update(Order entity)
        {
            _context.Orders.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(Order entity)
        {
            if (entity != null)
            {
                _context.Orders.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Remove(Order entity)
        {
            _context.Orders.Remove(entity);
            _context.SaveChanges();
        }
        public ICollection<Order> SearchProducts(string key)
        {
            return null;
        }
    }
}
