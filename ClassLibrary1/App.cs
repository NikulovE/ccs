using Bridge;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;

namespace ClassLibrary1
{
    public class App
    {
        public static void Main()
        {
            // Write a message to the Console
            Console.WriteLine("Welcome to Bridge.NET");

            // After building (Ctrl + Shift + B) this project, 
            // browse to the /bin/Debug or /bin/Release folder.

            // A new bridge/ folder has been created and
            // contains your projects JavaScript files. 

            // Open the bridge/index.html file in a browser by
            // Right-Click > Open With..., then choose a
            // web browser from the list

            // This application will then run in the browser.
        }
        public static string v(string password, string dataToEncrypt)
        {
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