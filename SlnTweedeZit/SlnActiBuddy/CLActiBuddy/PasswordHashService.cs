using System.Security.Cryptography;
using System.Text;

namespace CLActiBuddy
{
    public static class PasswordHashService
    {
        // https://stackoverflow.com/questions/12416249/hashing-a-string-with-sha256 
        public static string QuickHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash);
        }
    }
}
