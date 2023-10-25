using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public class CartRepository : ICartRepository<Cart>
    {

        private EcommerceContext _context;

        public CartRepository(EcommerceContext context)
        {
            _context = context;
        }

        public Cart Get(Guid Id)
        {
            return _context.Carts.FirstOrDefault(r => r.Id == Id);
        }

        public ICollection<Cart> GetAll()
        {
            return _context.Carts.ToList();
        }

        public void Add(Cart entity)
        {
            _context.Carts.Add(entity);
            _context.SaveChanges();
        }

        // Implement the GetCart method
        public Cart GetCart(int userId)
        {
            return _context.Carts.FirstOrDefault(r => r.UserId == userId);
        }
        public void Update(Cart entity)
        {
            _context.Carts.Update(entity);
            _context.SaveChanges();
        }
        public void Remove(Cart entity)
        {
            _context.Carts.Remove(entity);
            _context.SaveChanges();
        }
        public void Clear()
        {
            ICollection<Cart>ItemRemove = GetAll().ToList();
            _context.Carts.RemoveRange(ItemRemove);
            _context.SaveChanges();
        }

    } 
}
