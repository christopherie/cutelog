using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace CuteLog
{
    public sealed class AlertMessageManager : LogBase, IAlertMessage
    {
        private static readonly Lazy<AlertMessageManager> instance = new Lazy<AlertMessageManager>(() => new AlertMessageManager());

        private AlertMessageManager()
        {

        }

        public static AlertMessageManager GetInstance => instance.Value;

        public void MailAlert(Exception exception, string context, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            lock (lockObj)
            {
                Error error = new Error
                {
                    Context = context,
                    ErrorMessage = exception,
                    Site = Site,
                    MemberName = memberName,
                    FileName = fileName,
                    LineNumber = lineNumber
                };

                string emailTemplatePath = EmailTemplate;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(EmailAddress)
                };

                mailMessage.To.Add(EmailAddress);
                mailMessage.Body = string.Empty;

                using (StreamReader streamReader = new StreamReader(emailTemplatePath))
                {
                    mailMessage.Body = streamReader.ReadToEnd();
                }

                mailMessage.Body = mailMessage.Body.Replace("[time]", DateTime.Now.ToLongDateString());
                mailMessage.Body = mailMessage.Body.Replace("[site]", Site);
                mailMessage.Body = mailMessage.Body.Replace("[context]", error.Context);
                mailMessage.Body = mailMessage.Body.Replace("[message]", error.ErrorMessage.Message);
                mailMessage.Body = mailMessage.Body.Replace("[method]", error.MemberName);
                mailMessage.Body = mailMessage.Body.Replace("[filename]", error.FileName);
                mailMessage.Body = mailMessage.Body.Replace("[lineNumber]", error.LineNumber.ToString());
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Site Error";

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = MailMessageHost,
                    Port = MailMessagePort
                };

                smtpClient.Send(mailMessage);
            }
        }

        public void SlackAlert(Exception exception, string context, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            lock (lockObj)
            {
                Error error = new Error
                {
                    Context = context,
                    ErrorMessage = exception,
                    Site = Site,
                    MemberName = memberName,
                    FileName = fileName,
                    LineNumber = lineNumber
                };

                string alert = string.Empty;
                alert += "Site Error" + Environment.NewLine;
                alert += DateTime.Now.ToLongDateString() + Environment.NewLine;
                alert += "Site: " + error.Site + Environment.NewLine;
                alert += "Context: " + error.Context + Environment.NewLine;
                alert += "Message: " + error.ErrorMessage.Message + Environment.NewLine;
                alert += "Method: " + error.MemberName + Environment.NewLine;
                alert += "File: " + error.FileName + Environment.NewLine;
                alert += "Line: " + error.LineNumber.ToString() + Environment.NewLine;

                Payload payload = new Payload
                {
                    Channel = Channel,
                    Username = UserName,
                    AlertMessage = alert
                };

                string payloadJson = JsonConvert.SerializeObject(payload);

                using (WebClient webClient = new WebClient())
                {
                    NameValueCollection nameValueCollection = new NameValueCollection
                    {
                        ["payload"] = payloadJson
                    };
                    byte[] response = webClient.UploadValues(SlackUrlWithAccessToken, "POST", nameValueCollection);
                    string responseText = EncodingType.GetString(response);
                }
            }
        }
    }
}
