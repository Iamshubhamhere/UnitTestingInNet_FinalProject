using UnitTestingInNet_FinalProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UnitTestingInNet_FinalProject.Models.BusinessLayer
{
    public class BusinessLayer
    {
        private IRepository<Product> _productRepository;
        private IRepository<Order> _orderRepository;
        private ICartRepository<Cart> _cartRepository;
        private IRepository<ProductCart> _productCartRepository;
        private IRepository<Country> _countryRepository;
        public BusinessLayer(IRepository<Product> productRepository, ICartRepository<Cart> cartRepository, IRepository<ProductCart> productCartRepository,
            IRepository<Country> countryRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _productCartRepository = productCartRepository;
            _countryRepository = countryRepository;
        }

        public Product GetProductById(Guid Id)
        {
            Product productObtain = _productRepository.Get(Id);
            if (productObtain == null)
            {
                throw new ArgumentNullException("Item Not Found");
            }
            else
            {
                return productObtain;
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
            ICollection<Product>Products = _productRepository.SearchProducts(key);
            return Products;
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
        public OrderViewModel OrderToPlace()
        {
            // Retrieve the user's cart
            Cart selectedCart = _cartRepository.GetCart(1); // Change the user ID to match your logic

            if (selectedCart == null)
            {
                throw new InvalidOperationException("Cart is empty or not found.");
            }

            // Retrieve the items in the cart
            List<ProductCart> cartItems = _productCartRepository.GetAll()
                .Where(pc => pc.CartId == selectedCart.Id)
                .ToList();

            if (cartItems.Count == 0)
            {
                throw new InvalidOperationException("No items found in the cart.");
            }

            // Create a list of available countries
            List<Country> availableCountries = _countryRepository.GetAll().ToList();

            if (availableCountries.Count == 0)
            {
                throw new InvalidOperationException("No available countries found.");
            }

            // Calculate the total price of items in the cart
            decimal totalPrice = cartItems.Sum(item => item.Product.Price * item.ProductQuantity);

            // Create an OrderViewModel object to hold the data
            return new OrderViewModel
            {
                CartItems = cartItems,
                AvailableCountries = availableCountries,
                TotalPrice = totalPrice
            };
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
        public void ConfirmOrder(OrderViewModel orderViewModel)
        {
            // Retrieve the user's cart
            Cart userCart = _cartRepository.GetCart(1); // Change the user ID to match your logic

            if (userCart == null || orderViewModel.CartItems.Count == 0)
            {
                throw new InvalidOperationException("Cart is empty or not found.");
            }

            // Calculate the total ordered quantity
            int totalOrderedQuantity = orderViewModel.CartItems.Sum(item => item.ProductQuantity);

            // Create a new order
            Order newOrder = new Order
            {
                CartId = userCart.Id, // Assign the user's cart Id
                DestinationCountryId = orderViewModel.Order.DestinationCountryId, // This should be set by the user's selection in the view
                Address = orderViewModel.Order.Address, // From orderViewModel, user input
                MailingCode = orderViewModel.Order.MailingCode, // From orderViewModel, user input
                OrderedQuantity = totalOrderedQuantity, // Calculated total ordered quantity
                                                        // ... Set other properties as necessary
            };

            // Add the new order to the repository
            _orderRepository.Add(newOrder);

            // Clear the cart items
            foreach (var item in orderViewModel.CartItems)
            {
                _productCartRepository.Remove(item);
            }

            // Optionally, you might want to clear the Cart as well
            _cartRepository.Remove(userCart);

            // TODO: You might want to add transaction handling to ensure data consistency
        }


    }

}