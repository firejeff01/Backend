using System.Threading.Tasks;

namespace Backend.Api.ApplicationServices.Interfaces
{
    public interface IAuthAppService
    {
        Task<string?> LoginAsync(string email, string password);
    }
}
