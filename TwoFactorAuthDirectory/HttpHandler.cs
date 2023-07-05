using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace TwoFactorAuthDirectory
{
    [Serializable]
    public class HttpCodeException : Exception
    {
        public HttpCodeException(HttpStatusCode code) : base(String.Format("API did not return http code 200. Returned: {0}", code.ToString())) { }
    }

    public class HttpHandler
    {
        private String userAgent = "TwoFactorAuthDirectory/0.0.0.0";
        public String UserAgent
        {
            get { return this.userAgent; }
            set { this.userAgent = value; }
        }

        private String contentType = "application/json; charset=utf-8";
        public String ContentType
        {
            get { return this.contentType; }
            set { this.contentType = value; }
        }

        private int timeout = 10;
        public int Timeout
        {
            get { return this.timeout; }
            set { this.timeout = value; }
        }

        public HttpHandler()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);

            this.UserAgent = String.Format("{0}/{1}", fvi.ProductName, fvi.FileVersion);
        }

        internal async Task<String> FetchUrlAsync(String url)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };

            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("User-Agent", this.UserAgent);
            httpClient.DefaultRequestHeaders.Add("ContentType", this.ContentType);
            httpClient.Timeout = TimeSpan.FromSeconds(this.Timeout);

            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new HttpCodeException(response.StatusCode);
            }
        }
    }
}
