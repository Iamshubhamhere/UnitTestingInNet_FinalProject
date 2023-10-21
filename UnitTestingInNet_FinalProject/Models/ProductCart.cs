using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnitTestingInNet_FinalProject.Models
{
    public class ProductCart
    {
        [Key]
        public int Id { get; set; } 
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public Guid CartId { get; set;}
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        public int ProductQuantity { get; set; }
    }
}
