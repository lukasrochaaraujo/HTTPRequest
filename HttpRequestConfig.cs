using System;
using System.Text;

using HTTPRequest.Enums;

namespace HTTPRequest
{
    public class HttpRequestConfig
    {
        private string BasicAuthUsername;
        private string BasicAuthPassword;

        private string ApplicationName;
        private string ApplicationVersion;

        private string ApplicationOrigin;

        public IsoDateTimeType IsoDateTimeFormat { get; set; } = IsoDateTimeType.SPRING;

        public void AddBasicAuthorization(string user, string pass)
        {
            BasicAuthUsername = user;
            BasicAuthPassword = pass;
        }

        public string GetBasicAuthHash()
        {
            if (string.IsNullOrWhiteSpace(BasicAuthUsername) || string.IsNullOrWhiteSpace(BasicAuthPassword))
                throw new ArgumentNullException("BasicAuthorization data not provided (use: AddAuthorization(user, pass))");

            return "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(BasicAuthUsername + ":" + BasicAuthPassword));
        }

        public void AddApplicationInfo(string appname, string version)
        {
            ApplicationName = appname;
            ApplicationVersion = version;
        }

        public void AddApplicationUserInfo(string username, string origin)
        {
            ApplicationOrigin = $"{username}:{origin}";
        }

        public string GetApplicationInfo()
        {
            var sbInfo = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(ApplicationName))
                sbInfo.Append(ApplicationName);

            if (!string.IsNullOrWhiteSpace(ApplicationVersion))
                sbInfo.Append($"_v{ApplicationVersion}");

            if (!string.IsNullOrWhiteSpace(ApplicationOrigin))
                sbInfo.Append($"[{ApplicationOrigin}]");

            return sbInfo.ToString();
        }
    }
}
