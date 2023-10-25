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
        [TestMethod]
        public void GetProductById_WhenProductExists_ReturnsProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedProduct = new Product { Id = productId, Name = "TestProduct" };
            _mockProductRepository.Setup(repo => repo.Get(productId)).Returns(expectedProduct);

            // Act
            var result = _businessLayer.GetProductById(productId);

            // Assert
            Assert.AreEqual(expectedProduct, result);
        }
        [TestMethod]
        public void GetProductById_WhenProductDoesNotExist_ThrowsException()
        {
            // Arrange
            var nonExistentProductId = Guid.NewGuid();
            _mockProductRepository.Setup(repo => repo.Get(nonExistentProductId)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _businessLayer.GetProductById(nonExistentProductId));
            Assert.AreEqual("Item Not Found", exception.Message);
        }

        [TestMethod]
        public void GetProductById_WhenRepositoryThrowsUnexpectedException_PropagatesException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockProductRepository.Setup(repo => repo.Get(productId)).Throws(new Exception("Database error"));

            // Act & Assert
            var exception = Assert.ThrowsException<Exception>(() => _businessLayer.GetProductById(productId));
            Assert.AreEqual("Database error", exception.Message);
        }
    }
}