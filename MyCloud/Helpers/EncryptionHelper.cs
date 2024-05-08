using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace MyCloud.Helpers
{
    public class EncryptionHelper
    {
        private static byte[] HashUsername(string username)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(username));
                return hash;
            }
        }

        public static byte[] GenerateKeyFromUsername(string username)
        {
            byte[] hash = HashUsername(username);
            byte[] key = new byte[32];
            Array.Copy(hash, key, key.Length);
            return key;
        }

        public static byte[] GenerateIVFromUsername(string username)
        {
            byte[] hash = HashUsername(username);
            byte[] iv = new byte[16];
            Array.Copy(hash, 0, iv, 0, iv.Length);
            return iv;
        }
        public static void EncryptFile(string filename, string username)
        {
            filename = "D:\\MyCloud\\MyCloud\\wwwroot\\Files\\" + username + "\\" + filename + ".zip";
            string outputFileName = filename + "1";
            var key = GenerateKeyFromUsername(username);
            var iv = GenerateIVFromUsername(username);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var fsOutput = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var fsInput = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    fsInput.CopyTo(cryptoStream);
                }
            }
            File.Delete(filename);
        }

        public static void DecryptFile(string filename, string username)
        {
            filename = "D:\\MyCloud\\MyCloud\\wwwroot\\Files\\" + username + "\\" + filename + ".zip1";
            string outputFileName = filename.Remove(filename.Length - 1, 1);
            var key = GenerateKeyFromUsername(username);
            var iv = GenerateIVFromUsername(username);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var fsInput = new FileStream(filename, FileMode.Open, FileAccess.Read))
                using (var cryptoStream = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var fsOutput = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    cryptoStream.CopyTo(fsOutput);
                }
            }
        }
    }
}
