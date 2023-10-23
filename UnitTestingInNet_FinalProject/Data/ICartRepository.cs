using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public interface ICartRepository<T> where T : class
    {
        Cart Get(Guid Id);
        ICollection<Cart> GetAll();
        void Add(Cart entity);

        // Add a method for getting a cart by UserId
        Cart GetCart(int userId);
        void Update(T entity);
        void Remove(Cart entity);
    }
}
