using Known;

namespace SnsLite.Web.AjaxHandlers
{
    class ParamCheck
    {
        internal static bool CheckLogin(SnsUser user, out string message)
        {
            message = string.Empty;

            if (user == null || string.IsNullOrWhiteSpace(user.Id))
            {
                message = "请登录后再操作！";
                return false;
            }

            return true;
        }

        internal static bool GetParamArray(string param, int length, out string message, out string[] arr)
        {
            message = string.Empty;
            arr = null;

            if (string.IsNullOrWhiteSpace(param))
            {
                message = "参数不能为空！";
                return false;
            }

            arr = param.ToObject<string[]>();
            if (arr.Length != length)
            {
                message = "参数个数错误！";
                return false;
            }

            return true;
        }
    }
}