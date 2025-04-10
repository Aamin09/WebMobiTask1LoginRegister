using Task1LoginRegister.Interfaces;

namespace Task1LoginRegister.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment env;

        public ImageService(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public bool DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return false;

            try
            {
                var filePath = Path.Combine(env.WebRootPath, imageUrl.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return true;    
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file {imageUrl}: {ex.Message}");
                return false;
            }

        }

        public async Task<string> SaveImage(IFormFile file, string subfolder = "ProductImage")
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var relativePath = Path.Combine("Images", subfolder);
            var directory = Path.Combine(env.WebRootPath, relativePath);
            var filePath = Path.Combine(directory, fileName);

            // checking directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/Images/ProductsImage/{fileName}";
        }

    }
}
