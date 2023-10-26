using UnitTestingInNet_FinalProject.Data;
using UnitTestingInNet_FinalProject.Models.BusinessLayer;
using UnitTestingInNet_FinalProject.Models;
using Moq;

namespace StoreUnitTesting
{
    [TestClass]
    public class StoreUnitTest
    {
      
        private readonly BusinessLayer _businessLayer;
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<ICartRepository<Cart>> _mockCartRepository;
        private readonly Mock<IRepository<ProductCart>> _mockProductCartRepository;
        private readonly Mock<IRepository<Country>> _mockCountryRepository;
        private readonly Mock<IRepository<Order>> _mockOrderRepository;


        public StoreUnitTest()
        {
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockCartRepository = new Mock<ICartRepository<Cart>>();
            _mockProductCartRepository = new Mock<IRepository<ProductCart>>();
            _mockCountryRepository = new Mock<IRepository<Country>>();
            _mockOrderRepository = new Mock<IRepository<Order>>();  
       

            _businessLayer = new BusinessLayer(_mockProductRepository.Object, _mockCartRepository.Object,_mockProductCartRepository.Object,_mockCountryRepository.Object,_mockOrderRepository.Object /* other dependencies */);
        }
        #region Country 

        [TestMethod]
        public void GetAllCountries_ReturnsListOfCountries_WhenCountriesExist()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country { Name = "Country1" },
                new Country { Name = "Country2" },
            };
            _mockCountryRepository.Setup(repo => repo.GetAll()).Returns(countries);

            // Act
            var result = _businessLayer.GetAllCountries();

            // Assert
            Assert.AreEqual(countries.Count, result.Count);
        }

        [TestMethod]
        public void GetAllCountries_ThrowsArgumentNullException_WhenRepositoryReturnsNull()
        {
            // Arrange
            _mockCountryRepository.Setup(repo => repo.GetAll()).Returns((ICollection<Country>)null);

            // Act & Assert
            var exception = Assert.ThrowsException<NullReferenceException>(() => _businessLayer.GetAllCountries());
            Assert.AreEqual("No countries found in database", exception.Message);
        }

        [TestMethod]
        public void GetAllCountries_ReturnsEmptyList_WhenRepositoryReturnsEmptyList()
        {
            // Arrange
            var countries = new List<Country>(); // An empty list
            _mockCountryRepository.Setup(repo => repo.GetAll()).Returns(countries);

            // Act
            var result = _businessLayer.GetAllCountries();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAllCountries_ThrowsException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockCountryRepository.Setup(repo => repo.GetAll()).Throws(new InvalidOperationException("Database Error"));

            // Act & Assert
            var exception = Assert.ThrowsException<InvalidOperationException>(() => _businessLayer.GetAllCountries());
            Assert.AreEqual("Database Error", exception.Message);
        }
        #endregion
        #region Product 
        [TestMethod]
        public void GetAllProduct_ReturnsProducts_WhenProductsExist()
        {
            // Arrange
            var expectedProducts = new List<Product> { new Product(), new Product() }; // Dummy list of products
            _mockProductRepository.Setup(repo => repo.GetAll()).Returns(expectedProducts);

            // Act
            var result = _businessLayer.GetAllProduct();

            // Assert
            Assert.AreEqual(expectedProducts.Count, result.Count);
        }
        [TestMethod]
        public void GetAllProduct_ThrowsNullReferenceException_WhenRepositoryReturnsNull()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetAll()).Returns((ICollection<Product>)null);

            // Act & Assert
            var exception = Assert.ThrowsException<NullReferenceException>(() => _businessLayer.GetAllProduct());
            Assert.AreEqual("No product found in database", exception.Message);
        }
        [TestMethod]
        public void GetAllProduct_ReturnsEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var expectedProducts = new List<Product>(); // Empty list
            _mockProductRepository.Setup(repo => repo.GetAll()).Returns(expectedProducts);

            // Act
            var result = _businessLayer.GetAllProduct();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAllProduct_ReturnsEmptyCollection_WhenNoProductsAreStored()
        {
            // Arrange
            var expectedProducts = new List<Product>(); // An empty list
            _mockProductRepository.Setup(repo => repo.GetAll()).Returns(expectedProducts);

            // Act
            var result = _businessLayer.GetAllProduct();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        #endregion
        #region SearchingOfProduct
        [TestMethod]
        public void SearchProducts_ReturnsProducts_WhenKeyIsValid()
        {
            // Arrange
            var key = "sampleKey";
            var expectedProducts = new List<Product>() { new Product() /*... populate as necessary ...*/ };
            _mockProductRepository.Setup(repo => repo.SearchProducts(key)).Returns(expectedProducts);

            // Act
            var result = _businessLayer.SearchProducts(key);

            // Assert
            Assert.AreEqual(expectedProducts.Count, result.Count);
        }

        [TestMethod]
        public void SearchProducts_ReturnsNull_WhenKeyIsNullOrWhitespace()
        {
            // Arrange
            var key = "   "; // whitespace key

            // Act
            var result = _businessLayer.SearchProducts(key);

            // Assert
            Assert.IsNull(result);
        }
       
        [TestMethod]
        public void SearchProducts_ReturnsNull_WhenUnknownExceptionOccurs()
        {
            // Arrange
            var key = "someKey";
            _mockProductRepository.Setup(repo => repo.SearchProducts(key)).Throws(new Exception("Some unexpected exception"));

            // Act
            var result = _businessLayer.SearchProducts(key);

            // Assert
            Assert.IsNull(result);
        }


        #endregion

        #region GetCart
        [TestMethod]
        public void GetCartById_ReturnsCart_WhenCartExists()
        {
            // Arrange
            var testCartId = Guid.NewGuid();
            var expectedCart = new Cart();
            _mockCartRepository.Setup(x => x.Get(testCartId)).Returns(expectedCart);

            // Act
            var result = _businessLayer.GetCartById(testCartId);

            // Assert
            Assert.AreEqual(expectedCart, result);
        }

        [TestMethod]
        public void GetCartById_ThrowsArgumentNullException_WhenCartDoesNotExist()
        {
            // Arrange
            var testCartId = Guid.NewGuid();
            _mockCartRepository.Setup(x => x.Get(testCartId)).Returns((Cart)null);

            // Act & Assert
            var exception = Assert.ThrowsException<NullReferenceException>(() => _businessLayer.GetCartById(testCartId));
            Assert.AreEqual("Item Not Found", exception.Message);
        }


        [TestMethod]
        public void GetCartById_ThrowsArgumentException_WhenProvidedIdIsEmpty()
        {
            // Arrange
            Guid emptyGuid = Guid.Empty;

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => _businessLayer.GetCartById(emptyGuid));
            Assert.AreEqual("Provided ID is empty.", exception.Message);
        }

        [TestMethod]
        public void GetCartById_ThrowsException_WhenRepositoryThrowsUnexpectedException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            _mockCartRepository.Setup(x => x.Get(cartId)).Throws(new InvalidOperationException("Unexpected error."));

            // Act & Assert
            var exception = Assert.ThrowsException<InvalidOperationException>(() => _businessLayer.GetCartById(cartId));
            Assert.AreEqual("Unexpected error.", exception.Message);
        }
        [TestMethod]
        public void GetCartForUser_ReturnsExistingCart_IfCartExistsForUserId1()
        {
            // Arrange
            var mockCart = new Mock<Cart>();
            mockCart.Object.UserId = 1;

            _mockCartRepository.Setup(repo => repo.GetCart(1)).Returns(mockCart.Object);

            // Act
            var returnedCart = _businessLayer.GetCartForUser();

            // Assert
            Assert.AreEqual(mockCart.Object, returnedCart);
            _mockCartRepository.Verify(repo => repo.Add(It.IsAny<Cart>()), Times.Never);
        }

        [TestMethod]
        public void GetCartForUser_CreatesNewCart_IfNoCartExistsForUserId1()
        {
            // Arrange
            _mockCartRepository.Setup(repo => repo.GetCart(1)).Returns((Cart)null);
            _mockCartRepository.Setup(repo => repo.Add(It.IsAny<Cart>()));

            // Act
            var returnedCart = _businessLayer.GetCartForUser();

            // Assert
            Assert.IsNotNull(returnedCart);
            Assert.AreEqual(1, returnedCart.UserId);
            _mockCartRepository.Verify(repo => repo.Add(It.Is<Cart>(cart => cart.UserId == 1)), Times.Once);
        }

        #endregion

        #region ProductInCart
        [TestMethod]
        public void GetAllProductInCart_ReturnsAllProducts_WhenCartWithUserId1HasProducts()
        {
            // Arrange
            var mockCart = new Cart { Id = Guid.NewGuid() };  // assuming Id is of type Guid

            var mockProductCarts = new List<ProductCart>
    {
        new ProductCart { CartId = mockCart.Id, ProductId = Guid.NewGuid() },
        new ProductCart { CartId = mockCart.Id, ProductId = Guid.NewGuid() }
    };

            _mockCartRepository.Setup(repo => repo.GetCart(1)).Returns(mockCart);
            _mockProductCartRepository.Setup(repo => repo.GetAll()).Returns(mockProductCarts);


            // Setup the mockProductRepository to return dummy products for each ProductId
            foreach (var pc in mockProductCarts)
            {
                _mockProductRepository.Setup(repo => repo.Get(pc.ProductId)).Returns(new Product());
            }

            // Act
            var result = _businessLayer.GetAllProductInCart();

            // Assert
            Assert.AreEqual(mockProductCarts.Count, result.Count);
        }
        [TestMethod]
        public void GetAllProductInCart_ThrowsArgumentNullException_WhenCartWithUserId1DoesNotExist()
        {
            // Arrange
            _mockCartRepository.Setup(repo => repo.GetCart(1)).Returns((Cart)null);

            // Act & Assert
            var exception = Assert.ThrowsException<NullReferenceException>(() => _businessLayer.GetAllProductInCart());
         //Assert
            Assert.AreEqual("Selected Cart Not Found", exception.Message);
        }

        #endregion
        [TestMethod]
        public void AddToCart_ThrowsArgumentNullException_WhenProductIdIsNull()
        {
            Guid nullProductId = Guid.Empty;
            Guid cartId = Guid.NewGuid();

            var exception = Assert.ThrowsException<ArgumentNullException>(() => _businessLayer.AddToCart(nullProductId, cartId));
            Assert.AreEqual("Product ID not found. (Parameter 'productId')", exception.Message);
        }

        [TestMethod]
        public void AddToCart_ThrowsArgumentNullException_WhenCartIdIsNull()
        {
            Guid productId = Guid.NewGuid();
            Guid nullCartId = Guid.Empty;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => _businessLayer.AddToCart(productId, nullCartId));
            Assert.AreEqual("Cart ID not found. (Parameter 'cartId')", exception.Message);
        }

        [TestMethod]
        public void AddToCart_ThrowsArgumentException_WhenProductDoesNotExist()
        {
            Guid nonExistentProductId = Guid.NewGuid();
            Guid cartId = Guid.NewGuid();

            _mockProductRepository.Setup(repo => repo.Get(nonExistentProductId)).Returns((Product)null);

            var exception = Assert.ThrowsException<ArgumentException>(() => _businessLayer.AddToCart(nonExistentProductId, cartId));
            Assert.AreEqual("Product not found. (Parameter 'productId')", exception.Message);
        }

        [TestMethod]
        public void AddToCart_ThrowsArgumentException_WhenCartDoesNotExist()
        {
            Guid productId = Guid.NewGuid();
            Guid nonExistentCartId = Guid.NewGuid();

            _mockProductRepository.Setup(repo => repo.Get(productId)).Returns(new Product());
            _mockCartRepository.Setup(repo => repo.Get(nonExistentCartId)).Returns((Cart)null);

            var exception = Assert.ThrowsException<ArgumentException>(() => _businessLayer.AddToCart(productId, nonExistentCartId));
            Assert.AreEqual("Cart not found. (Parameter 'cartId')", exception.Message);
        }


        [TestMethod]
        public void IncreaseProductQuantity_ProductCartNotFound_ThrowsException()
        {
            Guid productCartId = Guid.NewGuid();
            _mockProductCartRepository.Setup(m => m.Get(productCartId)).Returns((ProductCart)null);

            Assert.ThrowsException<Exception>(() => _businessLayer.IncreaseProductQuantity(productCartId), "Product cart not found.");
        }

        [TestMethod]
        public void IncreaseProductQuantity_ProductOutOfStock_ThrowsException()
        {
            Guid productCartId = Guid.NewGuid();
            ProductCart pc = new ProductCart { ProductId = Guid.NewGuid() };
            Product p = new Product { AvailableQuantity = 0 };

            _mockProductCartRepository.Setup(m => m.Get(productCartId)).Returns(pc);
            _mockProductRepository.Setup(m => m.Get(pc.ProductId)).Returns(p);

            Assert.ThrowsException<Exception>(() => _businessLayer.IncreaseProductQuantity(productCartId), "Product is out of stock.");
        }

        [TestMethod]
        public void DecreaseProductQuantity_ProductCartNotFound_ThrowsException()
        {
            Guid productCartId = Guid.NewGuid();
            _mockProductCartRepository.Setup(m => m.Get(productCartId)).Returns((ProductCart)null);

            Assert.ThrowsException<Exception>(() => _businessLayer.DecreaseProductQuantity(productCartId), "Product cart not found.");
        }

        [TestMethod]
        public void DecreaseProductQuantity_ProductCartQuantityDecreasesBelowOne_RemovesProductCart()
        {
            Guid productCartId = Guid.NewGuid();
            ProductCart pc = new ProductCart { ProductId = Guid.NewGuid(), ProductQuantity = 1 };
            Product p = new Product { AvailableQuantity = 5 };

            _mockProductCartRepository.Setup(m => m.Get(productCartId)).Returns(pc);
            _mockProductRepository.Setup(m => m.Get(pc.ProductId)).Returns(p);

            _businessLayer.DecreaseProductQuantity(productCartId);

            _mockProductCartRepository.Verify(m => m.Remove(It.Is<ProductCart>(x => x == pc)), Times.Once());
            _mockProductRepository.Verify(m => m.Update(It.Is<Product>(x => x.AvailableQuantity == 6)), Times.Once());
        }
        [TestMethod]
        public void GetCountryById_ReturnsCountrySuccessfully_WhenCountryExists()
        {
            // Arrange
            var expectedCountry = new Country { Id = Guid.NewGuid(), Name = "TestCountry" };
            _mockCountryRepository.Setup(repo => repo.Get(expectedCountry.Id)).Returns(expectedCountry);

            // Act
            var result = _businessLayer.GetCountryById(expectedCountry.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCountry.Name, result.Name);
        }

        [TestMethod]
        public void GetCountryById_ThrowsException_WhenCountryDoesNotExist()
        {
            // Arrange
            var randomCountryId = Guid.NewGuid();
            _mockCountryRepository.Setup(repo => repo.Get(randomCountryId)).Returns((Country)null);

            // Act & Assert
            var exception = Assert.ThrowsException<Exception>(() => _businessLayer.GetCountryById(randomCountryId));
            Assert.AreEqual("Country not found.", exception.Message);
        }
        #region ConfirmOrderTest
        [TestMethod]
     
        public void ConfirmOrder_ThrowsException_WhenCountryIdIsEmpty()
        {
            var exception = Assert.ThrowsException<InvalidOperationException>(() => _businessLayer.ConfirmOrder(Guid.Empty));
            Assert.AreEqual("Invalid order data.", exception.Message);
        }

        [TestMethod]
        public void ConfirmOrder_ThrowsException_WhenCartNotFound()
        {
            _mockCartRepository.Setup(repo => repo.GetCart(It.IsAny<int>())).Returns((Cart)null);

            var exception = Assert.ThrowsException<InvalidOperationException>(() => _businessLayer.ConfirmOrder(Guid.NewGuid()));
            Assert.AreEqual("Cart is empty or not found.", exception.Message);
        }

        [TestMethod]
        public void ConfirmOrder_ThrowsException_WhenCountryNotFound()
        {
            _mockCartRepository.Setup(repo => repo.GetCart(It.IsAny<int>())).Returns(new Cart());
            _mockCountryRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).Returns((Country)null);
            _mockProductCartRepository.Setup(repo => repo.GetAll()).Returns(new List<ProductCart>()); 

            var exception = Assert.ThrowsException<InvalidOperationException>(() => _businessLayer.ConfirmOrder(Guid.NewGuid()));
            Assert.AreEqual("Selected country not found.", exception.Message);
        }

        [TestMethod]
        public void ConfirmAddress_AddsOrderToRepository()
        {
            // Arrange
            var testOrder = new Order
            {

                Id = Guid.NewGuid(),
                Address = "123 Test St."
            };

            // Act
            _businessLayer.ConfirmAddress(testOrder);

            // Assert
            _mockOrderRepository.Verify(repo => repo.Add(testOrder), Times.Once);
        }


        [TestMethod]
        public void GetAllOrder_ReturnsOrders_WhenOrdersExist()
        {
            // Arrange
            var orders = new List<Order>
        {
            new Order { },
            new Order { }
        };
            _mockOrderRepository.Setup(repo => repo.GetAll()).Returns(orders);

            // Act
            var result = _businessLayer.GetAllOrder();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // Asserting that we received two orders from the method
        }

        [TestMethod]
        public void GetAllOrder_ThrowsArgumentNullException_WhenNoOrdersExist()
        {
            // Arrange
            _mockOrderRepository.Setup(repo => repo.GetAll()).Returns((ICollection<Order>)null);

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _businessLayer.GetAllOrder());
            Assert.AreEqual("Value cannot be null. (Parameter 'No product found in database')", exception.Message);
        }


        [TestMethod]
        public void ClearCart_ClearsTheCartSuccessfully()
        {
            // Act
            _businessLayer.ClearCart();

            // Assert
            _mockCartRepository.Verify(repo => repo.Clear(), Times.Once); 
        }
        [TestMethod]
        public void ConfirmOrder_ReturnsValidOrderViewModel_WhenDataIsValid()
        {
            var testCountryId = Guid.NewGuid();
            var testCountry = new Country { ConversionRate = 1.5m, TaxRate = 0.2m, Name = "TestCountry" };
            var testCartId = Guid.NewGuid();
            var testCart = new Cart { Id = testCartId };
            var testProductCartList = new List<ProductCart>
{
    new ProductCart { CartId = testCartId, Product = new Product { Price = 100 }, ProductQuantity = 2 }
};


            _mockCartRepository.Setup(repo => repo.GetCart(It.IsAny<int>())).Returns(testCart);
            _mockProductCartRepository.Setup(repo => repo.GetAll())
                .Returns(testProductCartList.Where(pc => pc.CartId == testCart.Id).ToList());
            _mockCountryRepository.Setup(repo => repo.Get(testCountryId)).Returns(testCountry);

            var result = _businessLayer.ConfirmOrder(testCountryId);

            Assert.IsNotNull(result);
            Assert.AreEqual(2 * 100 * 1.5m * 1.2m + 2 * 100, result.TotalPrice); 

        }
        #endregion
    }
}