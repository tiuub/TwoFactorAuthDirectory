using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuthDirectory
{
    public class WebsiteTreeNode<String>
    {
        public String DomainLevel { get; private set; }
        public List<Website> Websites { get; private set; }
        public WebsiteTreeNode<String> Parent { get; set; }
        public ICollection<WebsiteTreeNode<String>> Children { get; private set; }

        public Boolean IsRoot
        {
            get { return Parent == null; }
        }

        public Boolean IsLeaf
        {
            get { return Children.Count == 0; }
        }

        public int Count
        {
            get
            {
                return this.Children.Sum(x => x.Count) + 1;
            }
        }

        public int Depth
        {
            get
            {
                if (this.IsLeaf)
                    return this.Level + 1;

                int deapest = 0;
                foreach (WebsiteTreeNode<String> node in this.Children)
                {
                    if (node.Depth > deapest)
                    {
                        deapest = node.Depth;
                    }
                }
                return deapest;
            }
        }

        public int Level
        {
            get
            {
                if (this.IsRoot)
                    return 0;
                return Parent.Level + 1;
            }
        }

        public WebsiteTreeNode(String domainLevel)
        {
            this.DomainLevel = domainLevel;

            this.Websites = new List<Website>();
            this.Children = new LinkedList<WebsiteTreeNode<String>>();
        }

        public WebsiteTreeNode<String> AddChild(String child)
        {
            WebsiteTreeNode<String> childNode = new WebsiteTreeNode<String>(child) { Parent = this };
            this.Children.Add(childNode);
            return childNode;
        }

        public WebsiteTreeNode<String> AddOrGetExistingChild(String child)
        {
            WebsiteTreeNode<String> childNode = FindSiteTreeNodeByDomainLevel(node => node.DomainLevel.Equals(child));
            return childNode != null ? childNode : AddChild(child);
        }

        public void AddWebsite(Website website)
        {
            this.Websites.Add(website);
        }

        public WebsiteTreeNode<String> FindSiteTreeNodeByDomainLevel(Func<WebsiteTreeNode<String>, bool> predicate)
        {
            return this.Children.FirstOrDefault(predicate);
        }

        public List<Website> GetAllWebsites()
        {
            return GetAllWebsites(_ => true);
        }

        public List<Website> GetAllWebsites(Func<Website, bool> predicate)
        {
            List<Website> output = new List<Website>();
            output.AddRange(this.Websites.Where(predicate));
            foreach (WebsiteTreeNode<String> node in this.Children)
            {
                output.AddRange(node.GetAllWebsites(predicate).Where(n => output.All(o => o != n)));
            }
            return output;
        }
    }
}
