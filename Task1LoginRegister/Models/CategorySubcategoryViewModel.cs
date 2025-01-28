namespace Task1LoginRegister.Models
{
    public class CategorySubcategoryViewModel
    {
        public Category Category { get; set; }
        public List<Subcategory> Subcategories { get; set; }=new List<Subcategory>();   
    }
}
