using System;
using System.Text;

using HTTPRequest.Enum;

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
                throw new ArgumentNullException("Os dados de autenticação não foram fornecidos (user AddAuthorization(usuario, senha))");

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
            StringBuilder sbInfo = new StringBuilder();

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
