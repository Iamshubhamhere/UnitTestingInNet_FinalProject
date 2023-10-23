namespace UnitTestingInNet_FinalProject.Models
{
    public class OrderViewModel
    {

        public Order Order { get; set; } // Include the Order model if necessary
        public List<ProductCart> CartItems { get; set; }
        public List<Country> AvailableCountries { get; set; }
        public decimal TotalPrice { get; set; }
        public string SelectedCountryName { get; set; }
        public decimal SelectedConversionRate { get; set; }
        public decimal SelectedTaxRate { get; set; }
        public int OrderedQuantity { get; set; }
    }
}
