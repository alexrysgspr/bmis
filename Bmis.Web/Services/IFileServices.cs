namespace Bmis.Web.Services
{
    public interface IFileServices
    {
        public Task Add(Stream file, string fileName);
    }
}
