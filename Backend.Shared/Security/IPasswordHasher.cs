namespace Backend.Shared.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password, int workFactor = 10);
        bool Verify(string hashed, string password);
    }
}
