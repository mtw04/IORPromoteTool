using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IORPromoteTool.Helper
{
    public class LFEncryptionProvider
    {
        private const int MAX_BYTE = 256;
        private const int SALT_LENGTH = 2;

        public LFEncryptionProvider()
        {
        }

        public string UnscrambleString(string str, byte[] scrambleKey)
        {
            int i = 0;
            TripleDESCryptoServiceProvider tripleDESprovider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            if (String.IsNullOrEmpty(str)) return str;

            byte[] buffer = System.Convert.FromBase64String(str);
            byte[] key = new byte[scrambleKey.Length];

            // The first two bytes are the salt
            byte[] salt = new byte[SALT_LENGTH];
            for (i = 0; i < SALT_LENGTH; i++)
            {
                salt[i] = buffer[i];
            }

            // mix in the salt to create the key
            for (i = 0; i < scrambleKey.Length; i++)
            {
                key[i] = (byte)((scrambleKey[i] + salt[i % SALT_LENGTH]) % MAX_BYTE);
            }

            // Hash the key with MD5
            byte[] hash = md5provider.ComputeHash(key);

            ICryptoTransform encrypto = tripleDESprovider.CreateDecryptor(hash, hash);

            MemoryStream ms = new MemoryStream(buffer, SALT_LENGTH, buffer.Length - SALT_LENGTH); // skip the first two bytes, which are the salt
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            byte[] decryptedBytes = new byte[ms.Length];
            cs.Read(decryptedBytes, 0, decryptedBytes.Length);
            string decryptedString = UTF8Encoding.UTF8.GetString(decryptedBytes);
            decryptedString = decryptedString.TrimEnd('\0');

            cs.Close();
            ms.Close();

            return decryptedString;
        }

        public string ScrambleString(string str, byte[] scrambleKey)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            int i = 0;
            TripleDESCryptoServiceProvider tripleDESprovider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();

            byte[] buffer = UTF8Encoding.UTF8.GetBytes(str);

            byte[] key = new byte[scrambleKey.Length];

            // Randomly generate two bytes to be the salt
            byte[] salt = new byte[SALT_LENGTH];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            // mix in the salt to create the key
            for (i = 0; i < scrambleKey.Length; i++)
            {
                key[i] = (byte)((scrambleKey[i] + salt[i % SALT_LENGTH]) % MAX_BYTE);
            }

            // Hash the key with MD5
            byte[] hash = md5provider.ComputeHash(key);

            ICryptoTransform encrypto = tripleDESprovider.CreateEncryptor(hash, hash);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(buffer, 0, buffer.Length);
            cs.FlushFinalBlock();

            // trim the '\0' bytes
            byte[] encryptedBuffer = ms.GetBuffer();
            byte[] byteOut = new byte[encryptedBuffer.Length + SALT_LENGTH]; // need room for the salt
            salt.CopyTo(byteOut, 0);
            encryptedBuffer.CopyTo(byteOut, SALT_LENGTH);

            int bufferEnd = (int)ms.Length + SALT_LENGTH; // add a couple for the salt

            // the final string is the salt + the encrypted string
            string encryptedString = System.Convert.ToBase64String(byteOut, 0, bufferEnd);

            cs.Close();
            ms.Close();
            return encryptedString;
        }

        // should change to use MS key file for security later
        public static byte[] LFEncrptionKey = new byte[] {0, 0, 115, 2,
										   222, 189, 223, 36,
										   53, 175, 111, 138,
										   211, 105, 12, 129};

        /// <summary>
        /// Wrapper to decrypt password using private key
        /// </summary>
        public static string Decrypt(string stringEncrypted)
        {
            LFEncryptionProvider encryptor = new LFEncryptionProvider();
            return encryptor.UnscrambleString(stringEncrypted, LFEncrptionKey);
        }

        public static string Encrypt(string stringToEncrypt)
        {
            LFEncryptionProvider encryptor = new LFEncryptionProvider();
            return encryptor.ScrambleString(stringToEncrypt, LFEncrptionKey);
        }
    }
}
