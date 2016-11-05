using Known.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace SnsLite.Web.ViewModels
{
    public enum ProfileType
    {
        [Description("基本设置")]
        Basic,
        [Description("安全设置")]
        Security
    }

    public enum ContentType
    {
        [Description("公司信息")]
        Company
    }

    public class TabPageModel : UserModel
    {
        public TabPageModel(SnsUser user, string title, string partialView, List<CodeTable> tabs) : base(user)
        {
            Title = title;
            PartialView = partialView;
            Tabs = tabs;
        }

        public string Title { get; private set; }
        public string PartialView { get; private set; }
        public List<CodeTable> Tabs { get; private set; }
        public object Data { get; set; }
    }
}