using System;
using System.Security.Cryptography;
using System.Text;

namespace Known.Inners
{
    sealed class Encryptor
    {
        public static string ToMd5(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static string Encrypt(string value, string password)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(password));
            var des = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB };
            var datas = Encoding.UTF8.GetBytes(value);
            var bytes = des.CreateEncryptor().TransformFinalBlock(datas, 0, datas.Length);
            return Convert.ToBase64String(bytes);
        }

        public static string Decrypt(string value, string password)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(password));
            var des = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB };
            var datas = Convert.FromBase64String(value);
            var bytes = des.CreateDecryptor().TransformFinalBlock(datas, 0, datas.Length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
