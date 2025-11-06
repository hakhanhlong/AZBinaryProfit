using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class StringHelper
    {
        public static int CountWords(string s)
        {
            MatchCollection collection = Regex.Matches(s, @"[\S]+");
            //MatchCollection collection = Regex.Matches(s, @"\w+");
            return collection.Count;
        }

        public static string UniqueKey(int maxSize)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        public static string MD5Hash(string str)
        {
            byte[] buffer = MD5.Create().ComputeHash(Encoding.Default.GetBytes(str.ToLower()));
            var builder = new StringBuilder();
            foreach (byte t in buffer)
            {
                builder.AppendFormat("{0:x2}", t);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Encrypts a string using the SHA256 (Secure Hash Algorithm) algorithm.
        /// Details: http://www.itl.nist.gov/fipspubs/fip180-1.htm
        /// This works in the same manner as MD5, providing however 256bit encryption.
        /// </summary>
        /// <param name="data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA256 algorithm.</returns>
        public static string SHA256Hash(string data)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Encrypts a string using the SHA384(Secure Hash Algorithm) algorithm.
        /// This works in the same manner as MD5, providing 384bit encryption.
        /// </summary>
        /// <param name="data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA384 algorithm.</returns>
        public static string SHA384Hash(string data)
        {
            SHA384 sha = new SHA384Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }


        /// <summary>
        /// Encrypts a string using the SHA512 (Secure Hash Algorithm) algorithm.
        /// This works in the same manner as MD5, providing 512bit encryption.
        /// </summary>
        /// <param name="data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA512 algorithm.</returns>
        public static string SHA512Hash(string data)
        {
            SHA512 sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }


        public static string ToSlug(string value)
        {
            // First, convert to lowercase and remove leading/trailing whitespace
            value = value.ToLowerInvariant().Trim();

            // Replace accented characters with their ASCII equivalents (optional, but good for SEO)
            // This is a simplified example; a more robust solution might use a library for transliteration.
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            // Replace spaces with hyphens
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            // Remove invalid characters (anything not a letter, digit, or hyphen)
            value = Regex.Replace(value, @"[^a-z0-9\-\s]", "", RegexOptions.Compiled);

            // Replace multiple hyphens with a single hyphen
            value = Regex.Replace(value, @"-{2,}", "-", RegexOptions.Compiled);

            // Trim hyphens from the beginning and end
            value = value.Trim('-');

            return value;
        }

    }
}
