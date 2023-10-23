using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using UnitTestingInNet_FinalProject.Data;
using UnitTestingInNet_FinalProject.Models;
using UnitTestingInNet_FinalProject.Models.BusinessLayer;

namespace UnitTestingInNet_FinalProject.Controllers
{
    public class StoreController : Controller
    {
        private BusinessLayer _businessLogicLayer;
        public StoreController(IRepository<Product> productRepository, ICartRepository<Cart> cartRepository, IRepository<ProductCart> productCartRepository,
            IRepository<Country> countryRepository)
        {
            _businessLogicLayer = new BusinessLayer(productRepository, cartRepository, productCartRepository, countryRepository);
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


        public IActionResult Search(string word)
        {
            try
            {
                ICollection<Product> getItems = _businessLogicLayer.SearchProducts(word);
                return View("Index",getItems);  
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        public IActionResult AddToCart(Guid ProductId) {

         
            // Check if the user has a cart or create a new one
            Cart cart = _businessLogicLayer.GetCartForUser();

            // Add the product to the user's cart
            _businessLogicLayer.AddToCart(ProductId, cart.Id);

            return RedirectToAction("Index");

        }

        public IActionResult Cart()
        {
            try
            {               
                ICollection<ProductCart> ProductsInCart = _businessLogicLayer.GetAllProductInCart();
                return View(ProductsInCart);
            }
            catch
            {
                throw new ArgumentNullException("No Items is found ");
            }
        }

        public IActionResult IncreaseQuantity(Guid productCartId)
        {
            try
            {
                _businessLogicLayer.IncreaseProductQuantity(productCartId);
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                // Handle the exception or show an error message
                return RedirectToAction("Cart");
            }
        }

        public IActionResult DecreaseQuantity(Guid productCartId)
        {
            try
            {
                _businessLogicLayer.DecreaseProductQuantity(productCartId);
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                // Handle the exception or show an error message

                return RedirectToAction("Cart");
            }
        }

        public IActionResult Order() 
        {
            try
            {
                var orderData = _businessLogicLayer.OrderToPlace();

                // Create an instance of OrderViewModel and populate its properties
                var viewModel = new OrderViewModel
                {
                    CartItems = orderData.CartItems,
                    AvailableCountries = orderData.AvailableCountries,
                    TotalPrice = orderData.TotalPrice
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("ORDER NOT found ");
            }
        }
        [HttpGet]
        public IActionResult ConfirmOrder()
        {
            try
            {
                OrderViewModel orderViewModel = _businessLogicLayer.OrderToPlace();
                return View(orderViewModel);
            }
            catch (InvalidOperationException ex)
            {
                // Handle the exception (e.g., log it and return an error view)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        public IActionResult ConfirmOrder(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _businessLogicLayer.ConfirmOrder(model);
                return RedirectToAction("OrderConfirmed"); // Redirect to a confirmation page
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it and return an error view)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult OrderConfirmed()
        {
            return View();
        }

    }
}
