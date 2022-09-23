using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwoFactorAuthDirectory
{
    public class TwoFactorAuthClient
    {
        private String apiUrl = "https://2fa.directory/api/v3/all.json";
        public String ApiUrl
        {
            get { return apiUrl; }
            set { this.apiUrl = value; }
        }

        private HttpHandler httpHandler = new HttpHandler();
        public HttpHandler HttpHandler
        {
            get { return httpHandler; }
            set { this.httpHandler = value; }
        }

        /// <summary>
        /// Fetches the whole list of websites from the API synchronously
        /// </summary>
        /// <exception cref="HttpCodeException"></exception>
        public List<Website> Fetch()
        {
            return FetchAsync().Result;
        }

        /// <summary>
        /// Fetches the whole list of websites from the API asynchronously
        /// </summary>
        /// <exception cref="HttpCodeException"></exception>
        public async Task<List<Website>> FetchAsync()
        {
            String response = await this.HttpHandler.FetchUrlAsync(this.ApiUrl);
            return TwoFactorAuthDirectory.Deserialize(response);
        }
    }

    public static class TwoFactorAuthDirectory 
    {
        /// <summary>
        /// Deserializes the json
        /// </summary>
        /// <param name="json">Json String</param>
        /// <returns>List of websites</returns>
        public static List<Website> Deserialize(String json)
        {
            return JsonConvert.DeserializeObject<List<Website>>(json, CustomConverter.Settings);
        }

        /// <summary>
        /// Serializes the list of websites and returns a json
        /// </summary>
        /// <param name="websites">List of websites</param>
        /// <returns>Serialized json string of websites</returns>
        public static String Serialize(List<Website> websites)
        {
            return JsonConvert.SerializeObject(websites, CustomConverter.Settings);
        }

        /// <summary>
        /// Searches for websites by given predicate
        /// </summary>
        /// <param name="websites">List of all websites</param>
        /// <param name="predicate">Func&lt;Website, bool&gt; of predicate</param>
        /// <returns>Websites which matches the given predicate</returns>
        public static List<Website> Find(List<Website> websites, Func<Website, bool> predicate)
        {
            return websites.Where(predicate).ToList();
        }

        /// <summary>
        /// Searches for websites by given domain
        /// </summary>
        /// <param name="websites">List of all websites</param>
        /// <param name="name">Name of the website</param>
        /// <param name="ignoreCase">Whether to search case-sensitive or not</param>
        /// <param name="regex">Whether to use regex or not</param>
        /// <returns>Websites which contain the given domain</returns>
        public static List<Website> FindByName(List<Website> websites, String name, bool ignoreCase = true, bool regex = true)
        {
            if (regex)
            {
                Regex rg = new Regex(name, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                return Find(websites, website => rg.IsMatch(website.Name));
            }
            else
            {
                return Find(websites, website => website.Name.Contains(name, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }

        /// <summary>
        /// Searches for websites by given domain
        /// </summary>
        /// <param name="websites">List of all websites</param>
        /// <param name="domain">String of domain</param>
        /// <returns>Websites which contain the given domain</returns>
        public static List<Website> FindByDomain(List<Website> websites, String domain)
        {
            return Find(websites, website => string.Equals(website.Domain, domain, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Searches for websites by given url
        /// </summary>
        /// <param name="websites">List of all websites</param>
        /// <param name="websites">List of all websites</param>
        /// <param name="url">String of url</param>
        /// <returns>Websites which contain the given url</returns>
        public static List<Website> FindByUrl(List<Website> websites, String url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri) || Uri.TryCreate("http://" + url, UriKind.Absolute, out uri))
                return FindByUrl(websites, uri);
            throw new UriFormatException("Given Url does not fit the Uri format.");
        }

        /// <summary>
        /// Searches for websites by given url
        /// </summary>
        /// <param name="websites">List of all websites</param>
        /// <param name="uri">String of url</param>
        /// <returns>Websites which contain the given url</returns>
        public static List<Website> FindByUrl(List<Website> websites, Uri uri)
        {
            return Find(websites, website => string.Equals(website.Domain, uri.Host, StringComparison.CurrentCultureIgnoreCase) ||
                                        website.Url.Contains(uri.Host, StringComparison.CurrentCultureIgnoreCase) ||
                                        website.Additional_domains.Contains(uri.Host, StringComparer.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Searches for websites by given types of two factor authentication
        /// </summary>
        /// <param name="websites">List of all websites</param>
        /// <param name="tfa">List&lt;String&gt; of types of two factor authentication</param>
        /// <returns>Websites which contains all of the given types of two factor authentication</returns>
        public static List<Website> FindByTfa(List<Website> websites, TfaTypes tfa)
        {
            return Find(websites, website => website.IsSupporting(tfa));
        }

        /// <summary>
        /// Searches for websites by given keywords
        /// </summary>
        /// <param name="websites">List of all websites</param>
        /// <param name="keywords">List&lt;String&gt; of keywords</param>
        /// <returns>Websites which contains all of the given keywords</returns>
        public static List<Website> FindByKeywords(List<Website> websites, List<String> keywords)
        {
            return Find(websites, website => keywords.All(keyword => website.Keywords.Contains(keyword)));
        }

        /// <summary>
        /// Searches for websites by given regions in ISO 3166-1 country codes
        /// </summary>
        /// <param name="websites">List of all websites</param>
        /// <param name="regions">List&lt;String&gt; of ISO 3166-1 country codes</param>
        /// <returns>Websites which contains all of the given country codes</returns>
        public static List<Website> FindByRegions(List<Website> websites, List<String> regions)
        {
            return Find(websites, website => regions.All(region => website.Regions.Contains(region)));
        }
    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// Serializes the list of websites and returns a json
        /// </summary>
        /// <returns>Serialized json string of websites</returns>
        public static String Serialize(this List<Website> websites)
        {
            return TwoFactorAuthDirectory.Serialize(websites);
        }

        /// <summary>
        /// Searches for websites by given domain
        /// </summary>
        /// <param name="domain">String of domain</param>
        /// <returns>Websites which contain the given domain</returns>
        public static List<Website> FindByName(this List<Website> websites, String name, bool ignoreCase = true, bool regex = false)
        {
            return TwoFactorAuthDirectory.FindByName(websites, name, ignoreCase, regex);
        }

        /// <summary>
        /// Searches for websites by given domain
        /// </summary>
        /// <param name="domain">String of domain</param>
        /// <returns>Websites which contain the given domain</returns>
        public static List<Website> FindByDomain(this List<Website> websites, String domain)
        {
            return TwoFactorAuthDirectory.FindByDomain(websites, domain);
        }

        /// <summary>
        /// Searches for websites by given url
        /// </summary>
        /// <param name="url">String of url</param>
        /// <returns>Websites which contain the given url</returns>
        public static List<Website> FindByUrl(this List<Website> websites, String url)
        {
            return TwoFactorAuthDirectory.FindByUrl(websites, url);
        }

        /// <summary>
        /// Searches for websites by given url
        /// </summary>
        /// <param name="uri">String of url</param>
        /// <returns>Websites which contain the given url</returns>
        public static List<Website> FindByUrl(this List<Website> websites, Uri uri)
        {
            return TwoFactorAuthDirectory.FindByUrl(websites, uri);
        }

        /// <summary>
        /// Searches for websites by given types of two factor authentication
        /// </summary>
        /// <param name="tfa">List&lt;String&gt; of types of two factor authentication</param>
        /// <returns>Websites which contains all of the given types of two factor authentication</returns>
        public static List<Website> FindByTfa(this List<Website> websites, TfaTypes tfa)
        {
            return TwoFactorAuthDirectory.FindByTfa(websites, tfa);
        }

        /// <summary>
        /// Searches for websites by given keywords
        /// </summary>
        /// <param name="keywords">List&lt;String&gt; of keywords</param>
        /// <returns>Websites which contains all of the given keywords</returns>
        public static List<Website> FindByKeywords(this List<Website> websites, List<String> keywords)
        {
            return TwoFactorAuthDirectory.FindByKeywords(websites, keywords);
        }

        /// <summary>
        /// Searches for websites by given regions in ISO 3166-1 country codes
        /// </summary>
        /// <param name="regions">List&lt;String&gt; of ISO 3166-1 country codes</param>
        /// <returns>Websites which contains all of the given country codes</returns>
        public static List<Website> FindByRegions(this List<Website> websites, List<String> regions)
        {
            return TwoFactorAuthDirectory.FindByRegions(websites, regions);
        }
    }
}
