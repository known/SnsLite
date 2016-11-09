using Known.Inners;
using System;
using System.Linq;

namespace Known
{
    /// <summary>
    /// 效用工具类。
    /// </summary>
    public sealed class Utils
    {
        /// <summary>
        /// 取得一个新GUID，去横线的小写字符串。
        /// </summary>
        public static string NewGuid
        {
            get { return Guid.NewGuid().ToString().ToLower().Replace("-", ""); }
        }

        /// <summary>
        /// 将基本类型对象转换成指定类型。
        /// </summary>
        /// <typeparam name="T">转换类型。</typeparam>
        /// <param name="value">需转换的对象。</param>
        /// <param name="defaultValue">转换默认值。</param>
        /// <returns>转换为指定类型的对象。</returns>
        public static T ConvertTo<T>(object value, T defaultValue = default(T))
        {
            if (value == null || value == DBNull.Value) return default(T);

            var valueString = value.ToString();
            var type = typeof(T);
            if (type == typeof(string)) return (T)Convert.ChangeType(valueString, type);

            valueString = valueString.Trim();
            if (valueString.Length == 0) return default(T);

            if (type.IsEnum) return (T)Enum.Parse(type, valueString, true);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            if (type == typeof(bool) || type == typeof(bool?))
                valueString = ",1,Y,YES,TRUE,".Contains(valueString.ToUpper()) ? "True" : "False";

            try
            {
                return (T)Convert.ChangeType(valueString, type);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 创建一个验证码图片。
        /// </summary>
        /// <param name="length">验证码字符长度。</param>
        /// <param name="code">验证码字符。</param>
        /// <returns>验证码图片字节。</returns>
        public static byte[] CreateCaptcha(int length, out string code)
        {
            return Image.CreateCaptcha(length, out code);
        }

        /// <summary>
        /// 将字符串进行MD5加密。
        /// </summary>
        /// <param name="value">明文字符串。</param>
        /// <returns>MD5加密字符串。</returns>
        public static string ToMd5(string value)
        {
            return Encryptor.ToMd5(value);
        }

        /// <summary>
        /// 将字符串通过密码进行加密。
        /// </summary>
        /// <param name="value">明文字符串。</param>
        /// <param name="password">加密密码。</param>
        /// <returns>加密字符串。</returns>
        public static string Encrypt(string value, string password)
        {
            return Encryptor.Encrypt(value, password);
        }

        /// <summary>
        /// 将加密字符串通过密码解密。
        /// </summary>
        /// <param name="value">加密字符串。</param>
        /// <param name="password">解密密码。</param>
        /// <returns>明文字符串。</returns>
        public static string Decrypt(string value, string password)
        {
            return Encryptor.Decrypt(value, password);
        }

        /// <summary>
        /// 根据IP地址获取该IP的所在地名称。
        /// </summary>
        /// <param name="ip">IP地址。</param>
        /// <returns>IP所在地名称。</returns>
        public static string GetIPAddressName(string ip)
        {
            return IPInfo.GetIPAddressName(ip);
        }

        /// <summary>
        /// 发送邮件。
        /// </summary>
        /// <param name="to">收件人邮箱，多人用分号（;）分割。</param>
        /// <param name="subject">邮件主题。</param>
        /// <param name="body">邮件内容。</param>
        public static void SendMail(string to, string subject, string body)
        {
            Mail.Send(to, subject, body);
        }

        public static string GetPagingSql(string sql, string[] orderFields, int pageSize, int pageIndex)
        {
            var orderBy = string.Join(",", orderFields.Select(f => string.Format("t1.{0}", f)));
            return string.Format("select top {0} t.* from (select t1.*,row_number() over (order by {1}) RowNo from ({2}) t1) t where t.RowNo>{3}", pageSize, orderBy, sql, pageSize * (pageIndex - 1));
        }
    }
}
