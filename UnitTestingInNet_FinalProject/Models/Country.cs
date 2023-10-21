using System.ComponentModel.DataAnnotations;

namespace UnitTestingInNet_FinalProject.Models
{
    public class Country
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Conversion rate must be greater than 0")]
        public decimal ConversionRate { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Tax rate must be between 0 and 1")]
        public decimal TaxRate { get; set; }

    }
}
