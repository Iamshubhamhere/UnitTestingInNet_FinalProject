using System.ComponentModel.DataAnnotations;

namespace UnitTestingInNet_FinalProject.Models
{
    public class Catalogue
    {
        [Key]
        public Guid Id { get; set; }
        List<Product> product { get; set; } = new List<Product>();

    }
}

