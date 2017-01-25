/// <permission cref="https://github.com/Kidel/Simple-AES">
/// Simple AES
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
/// </permission>

using System;

namespace SimpleAES
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Using keys from config to encrypt and decrypt the message 'secret message': ");
            SimpleAES aes = new SimpleAES();
            string m = "secret message";
            string enc = aes.EncryptToString(m);
            Console.WriteLine($"The encrypted message: {enc}");
            string dec = aes.DecryptString(enc);
            Console.WriteLine($"The decrypted message: {dec}");

            Console.WriteLine();

            Console.WriteLine("Please insert your key: ");
            string k = Console.ReadLine();
            Console.WriteLine("Please insert your vector: ");
            string v = Console.ReadLine();
            Console.WriteLine("Please insert your message to encrypt: ");
            string m2 = Console.ReadLine();

            SimpleAES aes2 = new SimpleAES(k, v);
            
            string enc2 = aes2.EncryptToString(m2);
            Console.WriteLine($"The encrypted message: {enc2}");
            string dec2 = aes2.DecryptString(enc2);
            Console.WriteLine($"The decrypted message: {dec2}");

            Console.ReadKey();
        }
    }
}
