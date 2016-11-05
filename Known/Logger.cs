using System;
using System.Diagnostics;
using System.Text;

namespace Known
{
    public interface ILogger
    {
        void Information(string message);
        void Information(string format, params object[] args);
        void Information(Exception exception, string format, params object[] args);

        void Warning(string message);
        void Warning(string format, params object[] args);
        void Warning(Exception exception, string format, params object[] args);

        void Error(string message);
        void Error(string format, params object[] args);
        void Error(Exception exception, string format, params object[] args);

        void TraceApi(string component, string method, TimeSpan timespan);
        void TraceApi(string component, string method, TimeSpan timespan, string properties);
        void TraceApi(string component, string method, TimeSpan timespan, string format, params object[] args);
    }

    public class Logger : ILogger
    {
        public void Information(string message)
        {
            Trace.TraceInformation(message);
        }

        public void Information(string format, params object[] args)
        {
            Trace.TraceInformation(format, args);
        }

        public void Information(Exception exception, string format, params object[] args)
        {
            Trace.TraceInformation(FormatExceptionMessage(exception, format, args));
        }

        public void Warning(string message)
        {
            Trace.TraceWarning(message);
        }

        public void Warning(string format, params object[] args)
        {
            Trace.TraceWarning(format, args);
        }

        public void Warning(Exception exception, string format, params object[] args)
        {
            Trace.TraceWarning(FormatExceptionMessage(exception, format, args));
        }

        public void Error(string message)
        {
            Trace.TraceError(message);
        }

        public void Error(string format, params object[] args)
        {
            Trace.TraceError(format, args);
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            Trace.TraceError(FormatExceptionMessage(exception, format, args));
        }

        public void TraceApi(string component, string method, TimeSpan timespan)
        {
            TraceApi(component, method, timespan, "");
        }

        public void TraceApi(string component, string method, TimeSpan timespan, string format, params object[] args)
        {
            TraceApi(component, method, timespan, string.Format(format, args));
        }

        public void TraceApi(string component, string method, TimeSpan timespan, string properties)
        {
            var message = string.Concat("Component:", component, ";Method:", method, ";Timespan:", timespan.ToString(), ";Properties:", properties);
            Trace.TraceInformation(message);
        }

        private static string FormatExceptionMessage(Exception exception, string format, object[] args)
        {
            // Simple exception formatting: for a more comprehensive version see  
            // http://code.msdn.microsoft.com/windowsazure/Fix-It-app-for-Building-cdd80df4 
            var sb = new StringBuilder();
            sb.Append(string.Format(format, args));
            sb.Append(" Exception: ");
            sb.Append(exception.ToString());
            return sb.ToString();
        }
    }
}
