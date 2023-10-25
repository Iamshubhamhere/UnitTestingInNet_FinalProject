using UnitTestingInNet_FinalProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnitTestingInNet_FinalProject.Models.ViewModel;

namespace UnitTestingInNet_FinalProject.Models.BusinessLayer
{
    public class BusinessLayer
    {
        private IRepository<Product> _productRepository;
        private ICartRepository<Cart> _cartRepository;
        private IRepository<ProductCart> _productCartRepository;
        private IRepository<Country> _countryRepository;
        private IRepository<Order> _orderRepository;
        public BusinessLayer(IRepository<Product> productRepository, ICartRepository<Cart> cartRepository, IRepository<ProductCart> productCartRepository,
            IRepository<Country> countryRepository, IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _productCartRepository = productCartRepository;
            _countryRepository = countryRepository;
            _orderRepository = orderRepository;
        }

        public Product GetProductById(Guid Id)
        {
            Product productObtain = _productRepository.Get(Id);
            if (productObtain == null)
            {
                throw new ArgumentNullException("Item Not Found");
            }
            return productObtain;
        }


        public ICollection<Country> GetAllCountries()
        {
            ICollection<Country> CountriesFound = _countryRepository.GetAll();
            if (CountriesFound == null)
            {
                throw new ArgumentNullException("No product found in database");
            }
            else
            {
                return CountriesFound;
            }
        }
        public ICollection<Product> GetAllProduct()
        {
            ICollection<Product> productFound = _productRepository.GetAll();
            if (productFound == null)
            {
                throw new ArgumentNullException("No product found in database");
            }
            else
            {
                return productFound;
            }
        }
        public ICollection<Product> SearchProducts(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Search key cannot be null or whitespace.", nameof(key));
                }

                if (_productRepository == null)
                {
                    throw new InvalidOperationException("_context is null. Please check your database context initialization.");
                }


                ICollection<Product> Products = _productRepository.SearchProducts(key);
                return Products;
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during product search: {ex.Message}");
             
                return null; 
            }
        }
        public Cart GetCartById(Guid Id)
        {
            Cart cartObtain = _cartRepository.Get(Id);
            if (cartObtain == null)
            {
                throw new ArgumentNullException("Item Not Found");
            }
            else
            {
                return cartObtain;
            }
        }

        public Cart GetCartForUser()
        {
            // Try to get the cart with UserId equal to 1
            Cart userCart = _cartRepository.GetCart(1);

            if (userCart == null)
            {
                // If the cart doesn't exist, create a new one with UserId 1
                userCart = new Cart
                {
                    UserId = 1
                };
                _cartRepository.Add(userCart); // Add the new cart to the repository
            }

            return userCart;
        }

        public ICollection<ProductCart> GetAllProductInCart()

        {
            Cart selectedCart = _cartRepository.GetCart(1);
            ICollection<ProductCart> ProductInCarts = _productCartRepository.GetAll().Where(pc => pc.CartId == selectedCart.Id).ToList();

            // Manually include the related Product entities.
            foreach (var productCart in ProductInCarts)
            {
                productCart.Product = _productRepository.Get(productCart.ProductId);
            }

            if (ProductInCarts == null)
            {
                throw new ArgumentNullException("Items Not Found");
            }
            else
            {
                return ProductInCarts;
            }
        }
   

        public void AddToCart(Guid productId, Guid cartId)
        {
            if (productId == null)
            {
                throw new ArgumentNullException(nameof(productId), "Product ID not found.");
            }

            if (cartId == null)
            {
                throw new ArgumentNullException(nameof(cartId), "Cart ID not found.");
            }

            var productToAdd = _productRepository.Get(productId);

            if (productToAdd == null)
            {
                throw new ArgumentException("Product not found.", nameof(productId));
            }

            var cartToAdd = _cartRepository.Get(cartId);

            if (cartToAdd == null)
            {
                throw new ArgumentException("Cart not found.", nameof(cartId));
            }

            if (productToAdd.AvailableQuantity > 0)
            {
                // Check if the product is already in the cart
                var existingCartItem = _productCartRepository.GetAll().FirstOrDefault(pc => pc.ProductId == productId && pc.CartId == cartId);

                if (existingCartItem != null)
                {
                    // Increase the quantity in the cart
                    existingCartItem.ProductQuantity++;
                }
                else
                {
                    // Add a new cart item with a quantity of 1
                    ProductCart cartItem = new ProductCart
                    {
                        ProductId = productId,
                        CartId = cartId,
                        ProductQuantity = 1
                    };
                    // Save the cartItem to the repository
                    _productCartRepository.Add(cartItem);
                }

                // Decrease the available quantity in the catalog page
                productToAdd.AvailableQuantity--;

                // Update the cart and product repositories
                _cartRepository.Update(cartToAdd);
                _productRepository.Update(productToAdd);
            }
            else
            {
                throw new InvalidOperationException("Product is out of stock.");
            }
        }

        public void IncreaseProductQuantity(Guid productCartId)
        {
            // Retrieve the ProductCart entity
            ProductCart productCart = _productCartRepository.Get(productCartId);

            if (productCart == null)
            {
                throw new Exception("Product cart not found.");
            }

            // Retrieve the related Product entity
            Product product = _productRepository.Get(productCart.ProductId);

            if (product.AvailableQuantity > 0)
            {
                product.AvailableQuantity--;
                productCart.ProductQuantity++;

                // Update the Product entity in your repository
                _productRepository.Update(product);

                // Update the ProductCart entity in your repository
                _productCartRepository.Update(productCart);
            }
            else
            {
                throw new Exception("Product is out of stock.");
            }
        }

        public void DecreaseProductQuantity(Guid productCartId)
        {
            // Retrieve the ProductCart entity
            ProductCart productCart = _productCartRepository.Get(productCartId);

            if (productCart == null)
            {
                throw new Exception("Product cart not found.");
            }

            // Retrieve the related Product entity
            Product product = _productRepository.Get(productCart.ProductId);

            if (productCart.ProductQuantity > 1)
            {
                product.AvailableQuantity++;
                productCart.ProductQuantity--;

                // Update the Product entity in your repository
                _productRepository.Update(product);

                // Update the ProductCart entity in your repository
                _productCartRepository.Update(productCart);
            }
            else
            {
                // Remove the product cart when the quantity becomes 0
                _productCartRepository.Remove(productCart);

                // Restore the available quantity of the product
                product.AvailableQuantity++;

                // Update the Product entity in your repository
                _productRepository.Update(product);
            }
        }
    

        public Country GetCountryById(Guid countryId)
        {
            Country country = _countryRepository.Get(countryId);
            if (country == null)
            {
                throw new Exception("Country not found.");
            }
            return country;
        }
        public OrderViewModel ConfirmOrder(Guid countryId)
        {
            // Ensure the OrderViewModel is valid and contains necessary data
            if (countryId == null )
            {
                throw new InvalidOperationException("Invalid order data.");
            }

            // Retrieve the user's cart
            Cart userCart = _cartRepository.GetCart(1); // This needs to match the logic you use to identify the user's cart
                                                        // Retrieve the items in the cart
            if (userCart == null)
            {
                throw new InvalidOperationException("Cart is empty or not found.");
            }
            List<ProductCart> cartItems = _productCartRepository.GetAll()
                .Where(pc => pc.CartId == userCart.Id)
                .ToList();


            // Retrieve the selected country's details
            Country selectedCountry = _countryRepository.Get(countryId);
            if (selectedCountry == null)
            {
                throw new InvalidOperationException("Selected country not found.");
            }
           
            // Calculate the total ordered quantity and total price
           
            decimal totalPrice = cartItems.Sum(item => item.Product.Price * item.ProductQuantity);
            decimal conversionRate = totalPrice * selectedCountry.ConversionRate;
            decimal taxRate = conversionRate * selectedCountry.TaxRate;
            decimal FinalTotalPrice = totalPrice + conversionRate + taxRate;


            OrderViewModel orderViewModel = new OrderViewModel()
            {
                CartItems = cartItems,
               
                TotalPrice = FinalTotalPrice,
                SelectedConversionRate = conversionRate,
                SelectedTaxRate = taxRate,
                SelectedCountryName = selectedCountry.Name,
                
            };
            return orderViewModel;
        }
        public void ConfirmAddress(Order order)
        {
            _orderRepository.Add(order);

        }

        public void ClearCart()
        {
            _cartRepository.Clear();

        }
        public ICollection<Order> GetAllOrder()
        {
            ICollection<Order> orderFound = _orderRepository.GetAll();
            if (orderFound == null)
            {
                throw new ArgumentNullException("No product found in database");
            }
            else
            {
                return orderFound;
            }
        }

    }

}