using System.ComponentModel.DataAnnotations;

namespace UnitTestingInNet_FinalProject.Models
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
     
        public HashSet<ProductCart> productCarts = new HashSet<ProductCart>();
      

    }
}
