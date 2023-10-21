using UnitTestingInNet_FinalProject.Data;

namespace UnitTestingInNet_FinalProject.Models.BusinessLayer
{
    public class BusinessLayer
    {
        private IRepository<Product> _productRepository;
        private IRepository<Order> _orderRepository;
        private IRepository<Cart> _cartRepository;
        public BusinessLayer(IRepository<Product> productRepository, IRepository<Cart> cartRepository ) {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
           
        }

        public Product GetProductById (Guid Id)
        {
            Product productObtain = _productRepository.Get(Id);  
            if(productObtain == null) {
                throw new ArgumentNullException("Item Not Found");
            }
            else
            {
                return productObtain;
            }
        }

        public ICollection<Product> GetAllProduct()
        {
        ICollection<Product>products = _productRepository.GetAll(); 
            if(products == null)
            {
                throw new ArgumentNullException("Items Not Found");
            }
            else
            {
                return products;
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
                ProductCart cartItem = new ProductCart
                {
                    ProductId = productId,
                    CartId = cartId,
                    ProductQuantity = 1// You can set the quantity as needed
                };
            }
            else
            {
                throw new InvalidOperationException("Product is out of stock.");
            }
        }
    }
}
