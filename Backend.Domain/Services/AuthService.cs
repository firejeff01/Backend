using Backend.Domain.Interfaces;
using Backend.Domain.Entities;
using System;
using System.Threading.Tasks;
using Backend.Shared.Security;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Backend.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtKey;
        private readonly string? _jwtIssuer;
        private readonly string? _jwtAudience;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthService>? _logger;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, string jwtKey, string? jwtIssuer = null, string? jwtAudience = null, ILogger<AuthService>? logger = null)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtKey = jwtKey;
            _jwtIssuer = jwtIssuer;
            _jwtAudience = jwtAudience;
            _logger = logger;
        }

        public async Task<string?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger?.LogDebug("User not found for email {email}", email);
                return null;
            }

            // Diagnostic logging to help investigate failing verification
            _logger?.LogDebug("Stored hashed password: {hash}", user.Password);
            _logger?.LogDebug("Incoming raw password: {pwd}", password);

            var ok = false;
            try
            {
                ok = _passwordHasher.Verify(user.Password, password);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while verifying password for {email}", email);
                return null;
            }

            if (!ok)
            {
                _logger?.LogDebug("Password verification failed for {email}", email);
                return null;
            }

            // create JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("sub", user.Id.ToString()), new Claim("email", user.Email) }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}