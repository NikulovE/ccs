using Shared;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var s = new Crypting.AES();
            var pass = "#base64Key#";
            //var x66 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pass));
            var key = "#base64Key#";
            //var iv= Convert.FromBase64String("#base64IV#");
            var p = v("shit", key);
        }

        public static string v(string password, string dataToEncrypt) {
            AesManaged aes = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;
            try
            {
                //Generate a Key based on a Password, Salt and HMACSHA1 pseudo-random number generator 
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes("guid"));

                //Create AES algorithm with 256 bit key and 128-bit block size 
                aes = new AesManaged();
                aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                rfc2898.Reset(); //needed for WinRT compatibility
                aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

                //Create Memory and Crypto Streams 
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                //Encrypt Data 
                byte[] data = Encoding.Unicode.GetBytes(dataToEncrypt);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return Convert.ToBase64String(memoryStream.ToArray());

            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
