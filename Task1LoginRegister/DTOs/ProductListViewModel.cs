using Microsoft.AspNetCore.Mvc.Rendering;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.DTOs
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Subcategories { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int? CategoryId { get; set; }
        public string SubcategoryIds { get; set; }
        public string SearchProductname { get; set; }
    }
}