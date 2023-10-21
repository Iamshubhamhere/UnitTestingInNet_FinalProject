using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnitTestingInNet_FinalProject.Models
{
    public class Order
    {
        public Guid Id { get; set; } 
        
        public int OrderedQuantity { get; set; }
        [Required]
        public Guid DestinationCountryId { get; set; }

        [ForeignKey("DestinationCountryId")]
        public Country DestinationCountry { get; set; }
        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(10)]
        public string MailingCode { get; set; }
        [Required]
        public Guid CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

    }
}
