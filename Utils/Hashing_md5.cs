using System.Security.Cryptography;
using System.Text;

namespace NzWalks.Utils
{
    public static class Hashing_md5
    {
        /// <summary>
        /// Computes MD5 hash of password using email as salt.
        /// </summary>
        /// <param name="email">User's email used as salt</param>
        /// <param name="password">Raw password</param>
        /// <returns>Hex-encoded hashed password</returns>
        public static string ComputeHash(string email, string password)
        {
            // First MD5 hash of the email (used as salt)
            byte[] tmpHash1 = MD5.HashData(Encoding.ASCII.GetBytes(email));
            string saltHex = Convert.ToHexString(tmpHash1);

            // Combine salt + password
            string saltedPassword = saltHex + password;

            // Second MD5 hash of the salted password
            byte[] tmpHash2 = MD5.HashData(Encoding.ASCII.GetBytes(saltedPassword));
            return Convert.ToHexString(tmpHash2);  // Returns all-uppercase hex
        }
    }
}
