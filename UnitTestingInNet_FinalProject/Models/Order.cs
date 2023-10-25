using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnitTestingInNet_FinalProject.Models
{
    public class Order
    {
        public Guid Id { get; set; } 
        
        public int OrderedQuantity { get; set; }
        [Required]
    
       
        [MaxLength(255)]
   
        public string Address { get; set; }

        [Required]
        [MaxLength(10)]
        public string MailingCode { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
