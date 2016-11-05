using System.Text.RegularExpressions;

namespace Known
{
    public class Validator
    {
        public static bool IsNumber(string input)
        {
            return Regex.IsMatch(input, @"^[0-9]*$");
        }

        public static bool IsEmail(string input)
        {
            return Regex.IsMatch(input, @"^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$");
        }

        public static bool IsUrl(string input)
        {
            return Regex.IsMatch(input, @"http(s)?://([/w-]+/.)+[/w-]+(/[/w- ./?%&=]*)?");
        }

        public static bool IsPhone(string input)
        {
            return Regex.IsMatch(input, @"^(\d{3,4}-)?\d{6,8}$");
        }

        public static bool IsMobile(string input)
        {
            return Regex.IsMatch(input, @"^[1]+[3,5]+\d{9}");
        }

        public static bool IsIDCard(string input)
        {
            return Regex.IsMatch(input, @"(^\d{18}$)|(^\d{15}$)");
        }

        public static bool IsPostalcode(string input)
        {
            return Regex.IsMatch(input, @"^\d{6}$");
        }
    }
}
