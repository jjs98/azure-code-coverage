using System.Threading.Tasks;

namespace CodeCoverage.Interfaces.Services
{
    public interface IHttpService
    {
        public Task<T> GetAsync<T>(string url) where T : class;
        public Task<T> PostAsync<T>(string url, T body) where T : class;
        public Task<T> PatchAsync<T>(string url, T body) where T : class;
    }
}
