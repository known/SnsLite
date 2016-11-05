using System.Net.Mail;
using System.Threading;

namespace Known.Inners
{
    sealed class Mail
    {
        private static readonly string adminEmail = Config.AppSetting("adminEmail");
        private static readonly string adminName = Config.AppSetting("adminName");

        public static void Send(string to, string subject, string body)
        {
            var t1 = new Thread(SendAsync);
            t1.Start(new string[] { to, subject, body });
        }

        private static void SendAsync(object emailInfo)
        {
            try
            {
                var paramArray = emailInfo as string[];
                if (paramArray != null)
                {
                    var to = paramArray[0];
                    var subject = paramArray[1];
                    var body = paramArray[2];

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(adminEmail, adminName);
                        var toItems = to.Split(';');
                        foreach (var item in toItems)
                        {
                            message.To.Add(new MailAddress(item));
                        }
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = false;

                        var mailClient = new SmtpClient();
                        mailClient.Send(message);
                    }
                }
            }
            catch { }
        }
    }
}
