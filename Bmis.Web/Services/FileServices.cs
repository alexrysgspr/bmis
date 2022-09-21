namespace Bmis.Web.Services
{
    public class FileServices : IFileServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileServices(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task Add(Stream file, string fileName)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await using var officialStream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            await file.CopyToAsync(officialStream);

        }
    }
}
