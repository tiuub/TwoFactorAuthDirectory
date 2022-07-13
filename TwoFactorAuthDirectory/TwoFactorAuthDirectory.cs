using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwoFactorAuthDirectory
{
    public class TwoFactorAuthClient
    {
        private String twoFactorAuthApiUrl = "https://2fa.directory/api/v3/all.json";
        public String TwoFactorAuthApiUrl
        {
            get { return twoFactorAuthApiUrl; }
            set { this.twoFactorAuthApiUrl = value; }
        }

        private WebsiteTreeNode<String> websites = new WebsiteTreeNode<String>("");
        public WebsiteTreeNode<String> Websites
        {
            get { return this.websites; }
            set { this.websites = value; }
        }

        public TwoFactorAuthClient()
        {
            
        }

        public async Task fetchAsync()
        {
            await fetchAsync(new HttpHandler());
        }

        public async Task fetchAsync(HttpHandler handler)
        {
            this.Websites = new WebsiteTreeNode<String>(""); // Defining root tree node

            String response = await handler.FetchUrlAsync(this.TwoFactorAuthApiUrl);
            List<Website> websites = JsonConvert.DeserializeObject<List<Website>>(response, CustomConverter.Settings);

            foreach (Website website in websites)
            {
                List<String> urls = new List<String>();
                urls.Add(website.Domain);
                if (website.Additional_domains != null)
                    urls.AddRange(website.Additional_domains);

                foreach (String url in urls)
                {
                    String host = new Uri(url).Host;
                    if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                        host = new Uri(url).DnsSafeHost;
                    String[] reversedDomainLevelArray = Utils.reversedDomainLevelArray(host);
                    WebsiteTreeNode<String> currentSiteTreeNode = this.Websites;
                    foreach (String domainLevel in reversedDomainLevelArray)
                    {
                        currentSiteTreeNode = currentSiteTreeNode.AddOrGetExistingChild(Utils.unescapeAsterisk(domainLevel));
                    }
                    currentSiteTreeNode.AddWebsite(website);
                }
            }
        }

        public List<Website> find(Func<Website, bool> predicate)
        {
            return this.Websites.GetAllWebsites(predicate);
        }

        public Website findFirstWebsiteByUrl(String url)
        {
            List<Website> websites = findAllWebsitesByUrl(url);
            if (websites.Count > 0)
            {
                return websites.First();
            }
            return null;
        }

        public List<Website> findAllWebsitesByUrl(String url)
        {
            return findAllWebsitesByUrl(new Uri(url));
        }

        public List<Website> findAllWebsitesByUrl(Uri uri)
        {
            List<Website> websites = new List<Website>();
            String[] reversedDomainLevelArray = Utils.reversedDomainLevelArray(uri.Host);
            WebsiteTreeNode<String> currentSiteTreeNode = this.Websites;
            int iteration = 0;
            foreach (String domainLevel in reversedDomainLevelArray)
            {
                iteration++;

                if (domainLevel.Equals("*"))
                {
                    websites.AddRange(currentSiteTreeNode.GetAllWebsites());
                    break;
                }

                currentSiteTreeNode = currentSiteTreeNode.FindSiteTreeNodeByDomainLevel(node => node.DomainLevel == domainLevel || node.DomainLevel == "*");

                if (currentSiteTreeNode == null)
                {
                    break;
                }

                if (iteration >= reversedDomainLevelArray.Count())
                {
                    websites.AddRange(currentSiteTreeNode.Websites);
                }

                if (currentSiteTreeNode.IsLeaf)
                {
                    break;
                }
            }
            return websites;
        }


        public List<Website> findAllWebsitesByTfa(TfaTypes tfa)
        {
            return this.Websites.GetAllWebsites(website => website.IsSupporting(tfa));
        }

        public List<Website> findAllWebsitesByKeyword(string keyword)
        {
            return this.Websites.GetAllWebsites(website => website.Keywords.Contains(keyword));
        }
    }
}
