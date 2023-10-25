using System.ComponentModel.DataAnnotations;

namespace UnitTestingInNet_FinalProject.Models.ViewModel
{
    public class OrderViewModel
    {


        public List<ProductCart> CartItems { get; set; }
        public ProductCart ProductCart { get; set; }

        public Guid OrderDestinationCountryId { get; set; }
        public decimal TotalPrice { get; set; }
        public string SelectedCountryName { get; set; }
        public decimal SelectedConversionRate { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(10)]
        public string MailingCode { get; set; }
        public decimal SelectedTaxRate { get; set; }
        public int OrderedQuantity { get; set; }
    }
}
