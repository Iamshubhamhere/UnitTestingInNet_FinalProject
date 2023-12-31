﻿using UnitTestingInNet_FinalProject.Data;
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


        public ICollection<Country> GetAllCountries()
        {
            ICollection<Country> countriesFound = _countryRepository.GetAll();

            if (countriesFound == null)
            {
                throw new NullReferenceException("No countries found in database");
            }

            return countriesFound;
        }


        public ICollection<Product> GetAllProduct()
        {
            ICollection<Product> productFound = _productRepository.GetAll();
            if (productFound == null)
            {
                throw new NullReferenceException("No product found in database");
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
                    throw new NullReferenceException("Search key cannot be null or whitespace.");
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
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Provided ID is empty.");
            }
            Cart cartObtain = _cartRepository.Get(Id);
            if (cartObtain == null)
            {
                throw new NullReferenceException("Item Not Found");
            }
            else
            {
                return cartObtain;
            }
        }

        public Cart GetCartForUser()
        {
          
            Cart userCart = _cartRepository.GetCart(1);

            if (userCart == null)
            {
               
                userCart = new Cart
                {
                    UserId = 1
                };
                _cartRepository.Add(userCart); 
            }

            return userCart;
        }

        public ICollection<ProductCart> GetAllProductInCart()

        {
            Cart selectedCart = _cartRepository.GetCart(1);
            if(selectedCart == null)
            {
                throw new NullReferenceException("Selected Cart Not Found");
            }
            ICollection<ProductCart> ProductInCarts = _productCartRepository.GetAll().Where(pc => pc.CartId == selectedCart.Id).ToList();

            
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
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId), "Product ID not found.");
            }

            if (cartId == Guid.Empty)
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
                
                var existingCartItem = _productCartRepository.GetAll().FirstOrDefault(pc => pc.ProductId == productId && pc.CartId == cartId);

                if (existingCartItem != null)
                {
                  
                    existingCartItem.ProductQuantity++;
                }
                else
                {
                   
                    ProductCart cartItem = new ProductCart
                    {
                        ProductId = productId,
                        CartId = cartId,
                        ProductQuantity = 1
                    };
                 
                    _productCartRepository.Add(cartItem);
                }

               
                productToAdd.AvailableQuantity--;

              
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
           
            ProductCart productCart = _productCartRepository.Get(productCartId);

            if (productCart == null)
            {
                throw new Exception("Product cart not found.");
            }

           
            Product product = _productRepository.Get(productCart.ProductId);

            if (product.AvailableQuantity > 0)
            {
                product.AvailableQuantity--;
                productCart.ProductQuantity++;

               
                _productRepository.Update(product);

               
                _productCartRepository.Update(productCart);
            }
            else
            {
                throw new Exception("Product is out of stock.");
            }
        }

        public void DecreaseProductQuantity(Guid productCartId)
        {
          
            ProductCart productCart = _productCartRepository.Get(productCartId);

            if (productCart == null)
            {
                throw new Exception("Product cart not found.");
            }

          
            Product product = _productRepository.Get(productCart.ProductId);

            if (productCart.ProductQuantity > 1)
            {
                product.AvailableQuantity++;
                productCart.ProductQuantity--;

               
                _productRepository.Update(product);

         
                _productCartRepository.Update(productCart);
            }
            else
            {
               
                _productCartRepository.Remove(productCart);

             
                product.AvailableQuantity++;

              
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
         
            if (countryId == Guid.Empty)
            {
                throw new InvalidOperationException("Invalid order data.");
            }

          
            Cart userCart = _cartRepository.GetCart(1); 
            if (userCart == null)
            {
                throw new InvalidOperationException("Cart is empty or not found.");
            }
            List<ProductCart> cartItems = _productCartRepository.GetAll()
                .Where(pc => pc.CartId == userCart.Id)
                .ToList();


           
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