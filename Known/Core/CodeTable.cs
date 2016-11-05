using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Known.Core
{
    public interface ICodeTableService
    {
        List<CodeTable> GetAllCodes();
    }

    public class CodeTable
    {
        public string Category { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int SortNo { get; set; }
        public bool Enabled { get; set; }

        public static void SetCache()
        {
            var cache = Container.Load<ICache>();
            var service = Container.Load<ICodeTableService>();

            if (cache == null || service == null)
                return;

            var codes = service.GetAllCodes();
            if (codes != null && codes.Count > 0)
            {
                cache.Set("CodeTable", codes);
            }
        }

        public static List<CodeTable> GetCodes(string category)
        {
            Check.Required(category, "category");

            var codes = GetCachedCodes();
            if (codes == null || codes.Count == 0)
                return null;

            return codes.Where(c => c.Category == category && c.Enabled).OrderBy(c => c.SortNo).ToList();
        }

        public static List<CodeTable> GetCodes(Type enumType)
        {
            var category = enumType.Name;
            var codes = new List<CodeTable>();
            var values = Enum.GetValues(enumType);
            foreach (Enum value in values)
            {
                var code = Convert.ToInt32(value);
                var name = EnumHelper.GetDescription(value);
                codes.Add(new CodeTable { Category = category, Code = code.ToString(), Name = name, Enabled = true, SortNo = code + 1 });
            }
            return codes;
        }

        class EnumHelper
        {
            internal static string GetDescription(Enum value)
            {
                var type = value.GetType();
                var name = Enum.GetName(type, value);
                var field = type.GetField(name);
                var attr = GetAttribute<DescriptionAttribute>(field, false);
                return attr != null ? attr.Description : name;
            }

            private static T GetAttribute<T>(MemberInfo member, bool inherit = true)
            {
                foreach (var attr in member.GetCustomAttributes(inherit))
                {
                    if (attr is T)
                    {
                        return (T)attr;
                    }
                }
                return default(T);
            }
        }

        public static CodeTable GetCode(string category, string code)
        {
            Check.Required(category, "category");
            Check.Required(code, "code");

            var codes = GetCodes(category);
            if (codes == null || codes.Count == 0)
                return null;

            return codes.FirstOrDefault(c => c.Code == code);
        }

        private static List<CodeTable> GetCachedCodes()
        {
            var cache = Container.Load<ICache>();
            if (cache == null)
                return null;

            return cache.Get<List<CodeTable>>("CodeTable");
        }
    }
}
