using System.Threading.Tasks;
using Backend.Api.ApplicationServices.Interfaces;
using Backend.Domain.Interfaces;

namespace Backend.Api.ApplicationServices
{
    public class AuthAppService : IAuthAppService
    {
        private readonly IAuthService _authService;

        public AuthAppService(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            return await _authService.AuthenticateAsync(email, password);
        }
    }
}
