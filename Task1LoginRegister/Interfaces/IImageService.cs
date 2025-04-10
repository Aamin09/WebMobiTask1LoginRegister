namespace Task1LoginRegister.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile file, string subfolder = "ProductImage");
        bool DeleteImage(string imageUrl);
    }
}
