using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SimpleAES
{
    /// <summary>
    /// Simple AES - Keys
    /// https://github.com/Kidel/Simple-AES
    /// 
    /// C♯ implementation of the Advanced Encryption Standard using System.Security.Cryptography.
    /// 
    /// Terms and conditions
    /// 
    /// Permission is granted to anyone to use this software for any purpose on any computer system, 
    /// and to redistribute it in any way, subject to the following restrictions:
    /// 
    /// 1) The author is not responsible for the consequences of use of this software, no matter how awful, even if they arise from defects in it.
    /// 2) Altered versions must be plainly marked as such, and must not be misrepresented (by explicit claim or omission) as being the original software.
    /// 3) This notice must not be removed or altered.
    /// </summary>
    static class Keys
    {
        private static string Key = ConfigurationManager.Load("Keys/Key");
        private static string Vector = ConfigurationManager.Load("Keys/Vector");

        private static UTF8Encoding UTFEncoder = new UTF8Encoding();

        /// <summary>
        /// Returns the bytes of the Key string specified in the static class Keys
        /// </summary>
        /// <returns>The bytes of the Key string</returns>
        public static byte[] KeyBytes()
        {
            if (Key == "key")
                throw new WarningException("You must change the default keys in config.xml");
            return GetKeyBytes(Key);
        }
        /// <summary>
        /// Returns the bytes of the Vector string specified in the static class Keys
        /// </summary>
        /// <returns>The bytes of the Vector string</returns>
        public static byte[] VectorBytes()
        {
            if (Vector == "vector")
                throw new WarningException("You must change the default keys in config.xml");
            return GetVectorBytes(Vector);
        }
        /// <summary>
        /// Returns the key of 32 bytes from the given string
        /// </summary>
        /// <param name="k">The desired key</param>
        /// <returns>The bytes of the key string</returns>
        public static byte[] GetKeyBytes(string k)
        {
            return getBytes(32, k);
        }
        /// <summary>
        /// Returns the vector of 16 bytes from the given string.
        /// </summary>
        /// <param name="k">The desired vector</param>
        /// <returns>The bytes of the vector string</returns>
        public static byte[] GetVectorBytes(string v)
        {
            return getBytes(16, v);
        }
        private static byte[] getBytes(int num, string k)
        {
            if (k == null || k.Length < 1)
                throw new Exception("Invalid key value");

            List<byte> bytes = new List<byte>(UTFEncoder.GetBytes(k));
            int i = 1;
            while (bytes.Count < 32)
            {
                List<byte> app = new List<byte>(UTFEncoder.GetBytes(shuffle(k, i)));
                bytes = bytes.Concat(app).ToList();
                ++i;
            }
            bytes = bytes.Take(num).ToList();
            return bytes.ToArray();
        }
        private static string shuffle(string str, int offset)
        {
            // Create new string from the reordered char array. 
            // Shuffle can't be completely random in order to make decryption possible, so it's done by using hash.
            return new string(str.ToCharArray()
                .OrderBy(s => ((s.GetHashCode() + offset) % 2) == 0).ToArray());
        }
    }
}
