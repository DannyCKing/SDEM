using System;
using System.IO;
using System.Security.Cryptography;

namespace Crypt
{
    public class PasswordConverter
    {
        private static readonly byte[] KeyByteArray = Convert.FromBase64String("Y7fauq8ha2f7gEaJ/+Al1lvY9UNJ0ffNQUhTqEhZsJI=");
        private static readonly byte[] IVByteArray = Convert.FromBase64String("V8Xlgs1OWO27zDYFrVI8Tw==");

        public PasswordConverter()
        {
            /*var rij = new RijndaelManaged();
            rij.GenerateKey();
            Console.WriteLine(Convert.ToBase64String(rij.Key));
            rij.GenerateIV();
            Console.WriteLine(Convert.ToBase64String(rij.IV));*/
        }

        public string Encrypt(string plainText)
        {
            using (var memStream = new MemoryStream())
            {
                using (var rij = new RijndaelManaged())
                {
                    rij.Padding = PaddingMode.PKCS7;
                    try
                    {
                        rij.Key = KeyByteArray;
                        rij.IV = IVByteArray;

                        using (ICryptoTransform transform = rij.CreateEncryptor(rij.Key, rij.IV))
                        {
                            using (var encryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write))
                            {
                                using (var writer = new StreamWriter(encryptStream))
                                {
                                    writer.Write(plainText);
                                }
                            }
                        }

                        return Convert.ToBase64String(memStream.ToArray());
                    }
                    finally
                    {
                        rij.Clear();
                    }
                }
            }
        }

        public string Decrypt(string encrypted)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);

            using (var rij = new RijndaelManaged())
            {
                rij.Padding = PaddingMode.PKCS7;
                try
                {
                    rij.Key = KeyByteArray;
                    rij.IV = IVByteArray;

                    using (ICryptoTransform transform = rij.CreateDecryptor(rij.Key, rij.IV))
                    {
                        using (var memStream = new MemoryStream(encryptedBytes))
                        {
                            memStream.Position = 0;
                            using (var decryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Read))
                            {
                                using (var reader = new StreamReader(decryptStream))
                                {
                                    return reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                finally
                {
                    rij.Clear();
                }
            }
        }
    }
}
