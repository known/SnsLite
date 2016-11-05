﻿using System.Collections.Generic;
using System.Web;

namespace Known.Web
{
    public sealed class WebUtils
    {
        public static string FormatHtml(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = HttpUtility.HtmlEncode(text);
            text = text.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
            text = text.Replace("\r\n", "<br/>");
            text = text.Replace("\r", "<br/>");
            text = text.Replace("\n", "<br/>");
            return text;
        }

        public static string AddUrlFragment(string rawUrl, string fragment)
        {
            if (string.IsNullOrWhiteSpace(rawUrl))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(fragment))
                return rawUrl;

            if (!rawUrl.Contains("?"))
                return rawUrl + "?" + fragment;

            var fragments = fragment.Split('=');
            if (!rawUrl.Contains(fragments[0] + "="))
                return rawUrl + "&" + fragment;

            var rtnUrls = rawUrl.Split('?');
            var rtnFragments = new List<string>();
            var rawFragments = rtnUrls[1].Split('&');
            foreach (var item in rawFragments)
            {
                if (item.StartsWith(fragments[0] + "="))
                    rtnFragments.Add(fragment);
                else
                    rtnFragments.Add(item);
            }
            return rtnUrls[0] + "?" + string.Join("&", rtnFragments);
        }

        public static string GetOSName(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return string.Empty;

            var osName = string.Empty;
            if (userAgent.Contains("NT 10.0"))
            {
                osName = "Windows 10";
            }
            else if (userAgent.Contains("NT 6.3"))
            {
                osName = "Windows 8.1";
            }
            else if (userAgent.Contains("NT 6.2"))
            {
                osName = "Windows 8";
            }
            else if (userAgent.Contains("NT 6.1"))
            {
                osName = "Windows 7";
            }
            else if (userAgent.Contains("NT 6.1"))
            {
                osName = "Windows 7";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osName = "Windows Vista/Server 2008";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                if (userAgent.Contains("64"))
                    osName = "Windows XP";
                else
                    osName = "Windows Server 2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osName = "Windows XP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osName = "Windows 2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osName = "Windows NT4";
            }
            else if (userAgent.Contains("Me"))
            {
                osName = "Windows Me";
            }
            else if (userAgent.Contains("98"))
            {
                osName = "Windows 98";
            }
            else if (userAgent.Contains("95"))
            {
                osName = "Windows 95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osName = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osName = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osName = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osName = "SunOS";
            }
            return osName;
        }
    }
}
