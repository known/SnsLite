using System.Net;

namespace Known.Inners
{
    sealed class IPInfo
    {
        class IPCheckResult
        {
            public int code { get; set; }
            public IPAdderssData data { get; set; }
        }

        class IPAdderssData
        {
            public string country { get; set; }
            public string country_id { get; set; }
            public string area { get; set; }
            public string area_id { get; set; }
            public string region { get; set; }
            public string region_id { get; set; }
            public string city { get; set; }
            public string city_id { get; set; }
            public string county { get; set; }
            public string county_id { get; set; }
            public string isp { get; set; }
            public string isp_id { get; set; }
            public string ip { get; set; }
        }

        public static string GetIPAddressName(string ip)
        {
            var ipName = DataCache.Get<string>(ip);
            if (string.IsNullOrWhiteSpace(ipName))
            {
                try
                {
                    var url = string.Format("http://ip.taobao.com/service/getIpInfo.php?ip={0}", ip);
                    var client = new WebClient();
                    var json = client.DownloadString(url);
                    var info = json.ToObject<IPCheckResult>();
                    if (info != null)
                    {
                        if (info.data.region == info.data.city)
                            ipName = string.Format("{0}{1}{2}", info.data.region, info.data.county, info.data.isp);
                        else
                            ipName = string.Format("{0}{1}{2}{3}", info.data.region, info.data.city, info.data.county, info.data.isp);

                        if (string.IsNullOrWhiteSpace(ipName))
                        {
                            ipName = info.data.country;
                        }
                        DataCache.Set(ip, ipName);
                    }
                }
                catch
                {
                }
            }
            return ipName;
        }
    }
}
