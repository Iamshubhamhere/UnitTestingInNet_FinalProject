using Microsoft.AspNetCore.Mvc.Rendering;

namespace UnitTestingInNet_FinalProject.Models.ViewModel
{
    public class CartViewModel
    {
        public ICollection<ProductCart> ProductCart { get; set; }
        public ICollection<SelectListItem> SelectedListItems { get; set; } = new List<SelectListItem>();
        public CartViewModel(ICollection<Country> Countries)
        {
            foreach (var country in Countries)
            {
                SelectedListItems.Add(new SelectListItem
                {
                    Text = country.Name,
                    Value = country.Id.ToString()
                });
            }
        }

    }
}
