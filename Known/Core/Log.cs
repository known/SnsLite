using Dapper;
using System;

namespace Known.Core
{
    public interface ILogService
    {
        void AddActionLog(ActionLog log);
        void AddVisitLog(VisitLog log);
    }

    public class LogService : ServiceBase, ILogService
    {
        public void AddActionLog(ActionLog log)
        {
            try
            {
                var sql = "insert into K_ActionLogs(UserName,ModuleName,ActionName,VisitTime,FinishTime) values(@UserName,@ModuleName,@ActionName,@VisitTime,@FinishTime)";
                connection.Execute(sql, log);
            }
            catch { }
        }

        public void AddVisitLog(VisitLog log)
        {
            try
            {
                if (log.RawUrl.Length > 500)
                {
                    log.RawUrl = log.RawUrl.Substring(0, 500);
                }

                var sql = "insert into K_VisitLogs(UserName,RawUrl,IPAddress,IPAddressName,OSName,Browser,BrowserVersion,VisitTime,FinishTime) values(@UserName,@RawUrl,@IPAddress,@IPAddressName,@OSName,@Browser,@BrowserVersion,@VisitTime,@FinishTime)";
                connection.Execute(sql, log);
            }
            catch { }
        }
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
