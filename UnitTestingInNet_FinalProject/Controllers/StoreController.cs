using Microsoft.AspNetCore.Mvc;
using UnitTestingInNet_FinalProject.Data;
using UnitTestingInNet_FinalProject.Models;
using UnitTestingInNet_FinalProject.Models.BusinessLayer;

namespace UnitTestingInNet_FinalProject.Controllers
{
    public class StoreController : Controller
    {
        private BusinessLayer _businessLogicLayer; 
        public StoreController(IRepository<Product> productRepository, IRepository<Cart> cartRepository)
        {
            _businessLogicLayer = new BusinessLayer(productRepository, cartRepository);
        }
      

        public IActionResult Index() {

            try
            {
                ICollection<Product> Products = _businessLogicLayer.GetAllProduct();
                return View(Products);  
            }
            catch
            {
                throw new ArgumentNullException("No Items is found ");
            }
            
            }
    }
}
