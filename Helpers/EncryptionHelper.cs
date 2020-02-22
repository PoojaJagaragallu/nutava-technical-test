using System.Security.Cryptography;
using System.Text;

namespace Nutava.Test.NumberToWord.Helpers
{
    public static class EncryptionHelper
    {
        /// <summary>
        /// Computes the plain text using SHA
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            StringBuilder returnValue = new StringBuilder();
            SHA1 sha1 = SHA1.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(plainText));
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }
    }
}
