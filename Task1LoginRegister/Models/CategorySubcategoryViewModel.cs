namespace Task1LoginRegister.Models
{
    public class CategorySubcategoryViewModel
    {
        public Category Category { get; set; }
        public List<Subcategory> Subcategories { get; set; }=new List<Subcategory>();
        public List<int> SubcategoryIdsToRemove { get; set; } = new List<int>(); // To store subcategory IDs to be removed
    }
}
