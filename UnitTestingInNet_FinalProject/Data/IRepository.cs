using System.Runtime.InteropServices;
using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public interface IRepository<T> where T : class
    {

        public T Get(Guid id);
        public ICollection<T> GetAll();
       
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
        public ICollection<T> SearchProducts(string Key);
    }
}
 