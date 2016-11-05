using System;

namespace Known
{
    public sealed class Check
    {
        public static bool IsNullOrEmpty(object value)
        {
            return value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString());
        }

        public static void Required(object value, string name)
        {
            if (IsNullOrEmpty(value))
                throw new ArgumentNullException(name);
        }

        public static void Throw(string message)
        {
            throw new Exception(message);
        }

        public static void Throw(string format, params object[] args)
        {
            var message = string.Format(format, args);
            Throw(message);
        }
    }
}
