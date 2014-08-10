using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace DM.Infrastructure.Util.CryptologyHelpers
{
    public static class CrypHelpers
    {
        #region MD5
        private static string MD5ProviderName = "MD5Cng";

        public static string MD5CreateHash(string plainText)
        {
            return Cryptographer.CreateHash(MD5ProviderName, plainText);
        }

        public static bool MD5CompareHash(string plainText, string hashedText)
        {
            return Cryptographer.CompareHash(MD5ProviderName, plainText, hashedText);
        }

        public static byte[] MD5CreateHash(byte[] plainText)
        {
            return Cryptographer.CreateHash(MD5ProviderName, plainText);
        }

        public static bool MD5CompareHash(byte[] plainText, byte[] hashedText)
        {
            return Cryptographer.CompareHash(MD5ProviderName, plainText, hashedText);
        }
        #endregion

        #region SHA
        private static string SHAProviderName = "SHA256Cng";

        public static string SHACreateHash(string plainText)
        {
            return Cryptographer.CreateHash(SHAProviderName, plainText);
        }

        public static bool SHACompareHash(string plainText, string hashedText)
        {
            return Cryptographer.CompareHash(SHAProviderName, plainText, hashedText);
        }

        public static byte[] SHACreateHash(byte[] plainText)
        {
            return Cryptographer.CreateHash(SHAProviderName, plainText);
        }

        public static bool SHACompareHash(byte[] plainText, byte[] hashedText)
        {
            return Cryptographer.CompareHash(SHAProviderName, plainText, hashedText);
        }
        #endregion

        #region Base64
        /// <summary>
        /// Convert a string to base64.
        /// </summary>
        /// <param name="plainText">String to be converted to base64</param>
        /// <returns>Base64 encoded value</returns>
        static public string EncryptBase64(string plainText)
        {
            return Convert.ToBase64String(new UnicodeEncoding().GetBytes(plainText));
        }

        /// <summary>
        /// Convert a string from base64.
        /// </summary>
        /// <param name="data">Base64 encoded data</param>
        /// <returns>String converted form base64</returns>
        static public string DecryptBase64(string cipherText)
        {
            byte[] arr = Convert.FromBase64String(cipherText);
            return new UnicodeEncoding().GetString(arr);
        }

        /// <summary>
        /// Convert a base64 string to a byte array.
        /// </summary>
        /// <param name="data">Base64 string</param>
        /// <returns>Binary data</returns>
        static public byte[] DecryptBase64Byte(string cipherText)
        {
            return Convert.FromBase64String(cipherText);
        }
        #endregion



        #region AES
        private static string AESProviderName = "AesManaged";

        public static string AESEncrypt(string plainText)
        {
            return Cryptographer.EncryptSymmetric(AESProviderName, plainText);
        }

        public static string AESDecrypt(string cipherText)
        {
            return Cryptographer.DecryptSymmetric(AESProviderName, cipherText);
        }

        public static byte[] AESEncrypt(byte[] plainText)
        {
            return Cryptographer.EncryptSymmetric(AESProviderName, plainText);
        }

        public static byte[] AESDecrypt(byte[] cipherText)
        {
            return Cryptographer.DecryptSymmetric(AESProviderName, cipherText);
        }
        #endregion
    }
}
