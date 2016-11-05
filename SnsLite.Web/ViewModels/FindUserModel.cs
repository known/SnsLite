using Known;
using Known.Core;
using SnsLite.Services;
using System.Collections.Generic;

namespace SnsLite.Web.ViewModels
{
    public class FindUserModel : Pagination
    {
        public FindUserModel(string region, string trade, string q, int pageIndex)
        {
            var companyService = ServiceFactory.GetService<ICompanyService>();
            var total = 0;
            Region = region;
            Trade = trade;
            Q = q;
            PageIndex = pageIndex;
            PageSize = Setting.PageSize;
            Regions = companyService.GetRegions();
            Trades = companyService.GetTrades();
            Companys = companyService.SearchCompanys(region, trade, q, pageIndex, out total);
            TotalCount = total;
            ResultInfo = string.Format("共搜索到{0}个相关公司！", total);
        }

        public string Region { get; private set; }
        public string Trade { get; private set; }
        public string Q { get; private set; }
        public string ResultInfo { get; private set; }
        public List<CodeTable> Regions { get; private set; }
        public List<CodeTable> Trades { get; private set; }
        public List<Company> Companys { get; private set; }
    }
}