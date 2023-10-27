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

        public decimal ConversionRate { get; set; }

        public decimal TaxRate { get; set; }

    }
}
