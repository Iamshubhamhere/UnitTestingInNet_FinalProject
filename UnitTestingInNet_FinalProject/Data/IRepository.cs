using System.Runtime.InteropServices;
using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public interface IRepository<T> where T : class
    {

        public T Get(Guid id);
        public ICollection<T> GetAll(); 
     
        public T AddItemToCart(Guid id, T item);    
    }
}
 