using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PortalCyberOffice._CyberOffice._Global
{
    public class Cryptography
    {
        #region Fields
        private static byte[] key = { };
        private static byte[] IV = { 38, 55, 206, 48, 28, 64, 20, 16 };
        private static string stringKey = "!566zz3a#KN";
        #endregion
        #region Public Methods
        public static string Encrypt(string text)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] byteArray = Encoding.UTF8.GetBytes(text);

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                    des.CreateEncryptor(key, IV), CryptoStreamMode.Write);

                cryptoStream.Write(byteArray, 0, byteArray.Length);
                cryptoStream.FlushFinalBlock();

                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string Decrypt(string text)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] byteArray = Convert.FromBase64String(text);

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                    des.CreateDecryptor(key, IV), CryptoStreamMode.Write);

                cryptoStream.Write(byteArray, 0, byteArray.Length);
                cryptoStream.FlushFinalBlock();

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string EncryptToURL(string text)
        {
            text = Encrypt(text);

            Char[] caracteres = text.ToCharArray();

            string chave = "";

            foreach (Char c in caracteres)
            {
                int codigo = (int)c;
                string valor = codigo.ToString("000");
                chave = chave + (chave.Length > 0 ? "-" : string.Empty);
                chave = chave + valor;
            }

            return chave;
        }
        public static string DecryptFromURL(string text)
        {
            string[] a2 = text.Split(new Char[] { '-' });

            string senha = "";

            foreach (string a3 in a2)
            {
                string a4 = a3.Replace("/", "");
                string letra = char.ConvertFromUtf32(Convert.ToInt32(a4));
                senha = senha + letra;
            }

            senha = Decrypt(senha);

            return senha;
        }
        #endregion
    }
}