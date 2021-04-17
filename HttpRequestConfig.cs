using System;
using System.Text;

using HTTPRequest.Types;

namespace HTTPRequest
{
    public class HttpRequestConfig
    {
        private string BasicAuthUsername;
        private string BasicAuthPassword;

        private string BearerAccessToken;

        private string ApplicationName;
        private string ApplicationVersion;

        private string ApplicationOrigin;

        public IsoDateTimeType IsoDateTimeFormat { get; set; } = IsoDateTimeType.ISO8601;
        public AuthorizationType Authorization { get; set; }

        public void AddBasicAuthorization(string user, string pass)
        {
            Authorization = AuthorizationType.BASIC;
            BasicAuthUsername = user;
            BasicAuthPassword = pass;
        }

        public void AddBearerAccessTokenAuthorization(string accessToken)
        {
            Authorization = AuthorizationType.BEARER;
            BearerAccessToken = accessToken;
        }

        public string GetBasicAuth()
        {
            if (string.IsNullOrWhiteSpace(BasicAuthUsername) || string.IsNullOrWhiteSpace(BasicAuthPassword))
                throw new ArgumentNullException($"{nameof(BasicAuthUsername)} and {nameof(BasicAuthPassword)} not configured on {nameof(HttpRequestConfig)}");

            return "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(BasicAuthUsername + ":" + BasicAuthPassword));
        }

        public string GetBearerAuth()
        {
            if (string.IsNullOrWhiteSpace(BearerAccessToken))
                throw new ArgumentNullException($"{nameof(BearerAccessToken)} not configured on {nameof(HttpRequestConfig)}");

            return "Bearer " + BearerAccessToken;
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
