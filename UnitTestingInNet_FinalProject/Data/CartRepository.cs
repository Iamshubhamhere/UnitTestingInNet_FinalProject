using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public class CartRepository : IRepository<Cart>
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
       
        public Cart AddItemToCart(Guid id, Cart item)
        {
            _context.Carts.Add(id, item);

        }
    }
}
