using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuthDirectory
{
    public class Utils
    {
        public static string escapeAsterisk(string url)
        {
            return url.Replace("*", "{ASTERISK}");
        }

        public static string unescapeAsterisk(string url)
        {
            return url.Replace("{ASTERISK}", "*");
        }

        public static string prettifyUrl(string url)
        {
            Uri uri = new Uri(url);
            return prettifyUrl(uri);
        }

        public static string prettifyUrl(Uri uri)
        {
            return uri.DnsSafeHost.ToLowerInvariant();
        }

        public static string[] reversedDomainLevelArray(string host)
        {
            return host.Split('.').Reverse().ToArray();
        }
    }
}
