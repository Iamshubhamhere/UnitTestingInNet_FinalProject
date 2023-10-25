using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using UnitTestingInNet_FinalProject.Data;
using UnitTestingInNet_FinalProject.Models;
using UnitTestingInNet_FinalProject.Models.BusinessLayer;
using UnitTestingInNet_FinalProject.Models.ViewModel;

namespace UnitTestingInNet_FinalProject.Controllers
{
    public class StoreController : Controller
    {
        private BusinessLayer _businessLogicLayer;
        public StoreController(IRepository<Product> productRepository, ICartRepository<Cart> cartRepository, IRepository<ProductCart> productCartRepository,
            IRepository<Country> countryRepository, IRepository<Order> orderRepository)
        {
            _businessLogicLayer = new BusinessLayer(productRepository, cartRepository, productCartRepository, countryRepository,orderRepository);
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
                ICollection<Country > Countries = _businessLogicLayer.GetAllCountries();
                CartViewModel cartView = new CartViewModel(Countries)
                {
                    ProductCart = ProductsInCart,
                    
                };
                return View(cartView);
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


        [HttpPost]
        public IActionResult Order(Guid OrderDestinationCountryId)
        {
            try
            {             
                var orderViewModel= _businessLogicLayer.ConfirmOrder(OrderDestinationCountryId);

                return View("Order", orderViewModel);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it and return an error view)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult OrderConfirmed(OrderViewModel order)
        {
            try
            {
                Order confirmOrder = new Order
                {
                    Address = order.Address,
                    MailingCode = order.MailingCode,
                    TotalPrice = order.TotalPrice,
                    OrderedQuantity = order.OrderedQuantity,    

                    
                };
                _businessLogicLayer.ConfirmAddress(confirmOrder);
                _businessLogicLayer.ClearCart();

                return RedirectToAction("Confirmation");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it and return an error view)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
        public IActionResult Confirmation()
        {
            return View();
        }
        public IActionResult AllOrder()
        {
            try
            {


                ICollection<Order> order = _businessLogicLayer.GetAllOrder();
                return View(order);
            }
            catch
            {
                throw new ArgumentNullException("No Items is found ");
            }
        }
    }
}
