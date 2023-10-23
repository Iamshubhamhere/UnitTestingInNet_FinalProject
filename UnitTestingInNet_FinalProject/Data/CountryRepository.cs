using UnitTestingInNet_FinalProject.Models;

namespace UnitTestingInNet_FinalProject.Data
{
    public class CountryRepository : IRepository<Country>
    {
        private EcommerceContext _context;

        public CountryRepository(EcommerceContext context)
        {
            _context = context;
        }

        public Country Get(Guid Id)
        {
            return _context.Countries.FirstOrDefault(r => r.Id == Id);
        }

        public ICollection<Country> GetAll()
        {
            return _context.Countries.ToList();
        }

        public void Add(Country entity)
        {
            _context.Countries.Add(entity);
            _context.SaveChanges();
        }

        // Implement the GetCart method
        public Country GetCart(Guid CountryId)
        {
            return _context.Countries.FirstOrDefault(r => r.Id == CountryId);
        }
        public void Update(Country entity)
        {
            _context.Countries.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(Country entity)
        {
            if (entity != null)
            {
                _context.Countries.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Remove(Country entity)
        {
            _context.Countries.Remove(entity);
            _context.SaveChanges();
        }
        public ICollection<Country> SearchProducts(string key)
        {
            ICollection<Country> search = _context.Countries.Where(pr => pr.Name.Contains(key)).OrderBy(pr => pr.Name).ToList();
            return search;
        }
    }
}
