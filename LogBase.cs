using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Text;

namespace CuteLog
{
    public abstract class LogBase
    {
        protected readonly object lockObj = new object();

        [JsonProperty("channel")]
        public string Channel { get; } = ConfigurationManager.AppSettings["CLogSlackChannel"];
        public string CustomTimestamp { get; } = ConfigurationManager.AppSettings["CLogCustomTimestamp"];
        public string DbConnectionString { get; } = ConfigurationManager.ConnectionStrings["CLogConnectionString"].ConnectionString;
        public static string EmailAddress { get; } = ConfigurationManager.AppSettings["CLogEmailAddress"];
        public static string EmailTemplate { get; } = ConfigurationManager.AppSettings["CLogEmailTemplate"];
        public Encoding EncodingType { get; } = new UTF8Encoding();
        public string MailMessageHost { get; } = ConfigurationManager.AppSettings["CLogMailMessageHost"];
        public int MailMessagePort { get; } = Convert.ToInt16(ConfigurationManager.AppSettings["CLogMailMessagePort"]);
        public string Site { get; } = ConfigurationManager.AppSettings["CLogSite"];
        public string SlackUrlWithAccessToken { get; } = ConfigurationManager.AppSettings["CLogSlackUrlWithAccessToken"];

        [JsonProperty("username")]
        public string UserName { get; } = ConfigurationManager.AppSettings["CLogSlackUser"];
    }
}
