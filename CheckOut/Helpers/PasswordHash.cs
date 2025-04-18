using System.Security.Cryptography;
using System.Text;

namespace CheckOut.Helpers
{
    public static class PasswordHash
    {
        public static string Hash(string password)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}