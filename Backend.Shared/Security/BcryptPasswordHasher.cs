using BCrypt.Net;

namespace Backend.Shared.Security
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password, int workFactor = 10)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
        }

        public bool Verify(string hashed, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }
    }
}
