using System;

namespace Known.Core
{
    public interface ILogService
    {
        void AddActionLog(ActionLog log);
        void AddVisitLog(VisitLog log);
    }

    public class VisitLog
    {
        public string UserName { get; set; }
        public string RawUrl { get; set; }
        public string IPAddress { get; set; }
        public string IPAddressName { get; set; }
        public string OSName { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public DateTime VisitTime { get; set; }
        public DateTime FinishTime { get; set; }
    }

    public class ActionLog
    {
        public string UserName { get; set; }
        public string ModuleName { get; set; }
        public string ActionName { get; set; }
        public DateTime VisitTime { get; set; }
        public DateTime FinishTime { get; set; }
    }
}
