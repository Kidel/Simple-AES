using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace SimpleAES
{
    /// <summary>
    /// Simple AES
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
    public class SimpleAES
    {
        private byte[] Key;
        private byte[] Vector;

        private ICryptoTransform EncryptorTransform;
        private ICryptoTransform DecryptorTransform;

        private UTF8Encoding UTFEncoder = new UTF8Encoding();

        /// <summary>
        /// Initializes SimpleAES using k and v from Keys static members.
        /// </summary>
        public SimpleAES()
        {
            Key = Keys.KeyBytes();
            Vector = Keys.VectorBytes();

            initializeEncription();
        }
        /// <summary>
        /// Initializes SimpleAES using given k and v strings.
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        public SimpleAES(string k, string v)
        {
            Key = Keys.GetKeyBytes(k);
            Vector = Keys.GetVectorBytes(v);

            initializeEncription();
        }
        /// <summary>
        /// Initializes SimpleAES using given k and v byte arrays (32 and 16 bytes respectively).
        /// </summary>
        /// <param name="k">32 bytes</param>
        /// <param name="v">16 bytes</param>
        public SimpleAES(byte[] k, byte[] v)
        {
            Key = k;
            Vector = v;

            initializeEncription();
        }
        private void initializeEncription()
        {
            // The encryption method
            RijndaelManaged rm = new RijndaelManaged();
            rm.Padding = PaddingMode.PKCS7;

            // Creates an encryptor and a decryptor using encryption method, key and vector.
            EncryptorTransform = rm.CreateEncryptor(Key, Vector);
            DecryptorTransform = rm.CreateDecryptor(Key, Vector);
        }

        /// <summary>
        /// Encrypts a string to a sanitized string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string EncryptToString(string str)
        {
            return byteArrToString(Encrypt(str));
        }
        /// <summary>
        /// Encrypts a string to a byte array.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public byte[] Encrypt(string str)
        {
            byte[] bytes = UTFEncoder.GetBytes(str);

            return EncryptByteToByte(bytes);
        }
        /// <summary>
        /// Encrypts a byte array to another byte array.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] EncryptByteToByte(byte[] bytes)
        {
            // Used to stream the data in and out of the CryptoStream.
            MemoryStream memoryStream = new MemoryStream();

            // Write the decrypted value to the encryption stream
            CryptoStream cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();

            // Read encrypted value back out of the stream
            memoryStream.Position = 0;
            byte[] encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);

            // Clean up.
            cs.Close();
            memoryStream.Close();

            return encrypted;
        }

        /// <summary>
        /// Decrypts a sanitized string to its original string message.
        /// </summary>
        /// <param name="encryptedStr"></param>
        /// <returns></returns>
        public string DecryptString(string encryptedStr)
        {
            return Decrypt(strToByteArray(encryptedStr));
        }
        /// <summary>
        /// Decrypts a byte array to its original string message.
        /// </summary>
        /// <param name="encryptedVal"></param>
        /// <returns></returns>
        public string Decrypt(byte[] encryptedVal)
        {
            return UTFEncoder.GetString(DecryptByteToByte(encryptedVal));
        }
        /// <summary>
        /// Decrypts a byte array to its original unencrypted bytes.
        /// </summary>
        /// <param name="encryptedVal"></param>
        /// <returns></returns>
        public byte[] DecryptByteToByte(byte[] encryptedVal)
        {
            // Write the encrypted value to the decryption stream
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(encryptedVal, 0, encryptedVal.Length);
            decryptStream.FlushFinalBlock();

            // Read the decrypted value from the stream.
            encryptedStream.Position = 0;
            Byte[] decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();

            return decryptedBytes;
        }

        private byte[] strToByteArray(string str)
        {
            if (str == null || str.Length < 1)
                throw new Exception("Invalid string value");

            byte val;
            byte[] byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            }
            while (i < str.Length);
            return byteArr;
        }
        public string byteArrToString(byte[] byteArr)
        {
            byte val;
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                val = byteArr[i];
                if (val < (byte)10)
                    tempStr += "00" + val.ToString();
                else if (val < (byte)100)
                    tempStr += "0" + val.ToString();
                else
                    tempStr += val.ToString();
            }
            return tempStr;
        }
    }
}
