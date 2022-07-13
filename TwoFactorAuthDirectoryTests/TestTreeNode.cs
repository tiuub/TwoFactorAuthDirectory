using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoFactorAuthDirectory;

namespace TwoFactorAuthDirectoryTests
{
    [TestClass]
    public class TestTreeNode
    {
        [TestMethod]
        public void IsRoot()
        {
            WebsiteTreeNode<String> root = new WebsiteTreeNode<String>(".") { Parent = null };
            WebsiteTreeNode<String> de = root.AddChild("de");

            Assert.AreEqual(de.IsRoot, false);
            Assert.AreEqual(root.IsRoot, true);
        }

        [TestMethod]
        public void IsLeaf()
        {
            WebsiteTreeNode<String> root = new WebsiteTreeNode<String>(".") { Parent = null };
            WebsiteTreeNode<String> de = root.AddChild("de");
            WebsiteTreeNode<String> yt = de.AddChild("youtube");

            Assert.AreEqual(de.IsLeaf, false);
            Assert.AreEqual(yt.IsLeaf, true);
        }

        [TestMethod]
        public void Level()
        {
            WebsiteTreeNode<String> root = new WebsiteTreeNode<String>(".") { Parent = null };
            WebsiteTreeNode<String> de = root.AddChild("de");
            WebsiteTreeNode<String> yt = de.AddChild("youtube");

            Assert.AreEqual(yt.Level, 2);
        }

        [TestMethod]
        public void FindSiteTreeNode()
        {
            WebsiteTreeNode<String> root = new WebsiteTreeNode<String>(".") { Parent = null };
            WebsiteTreeNode<String> de = root.AddChild("de");
            WebsiteTreeNode<String> uk = root.AddChild("uk");
            WebsiteTreeNode<String> io = root.AddChild("io");

            Assert.AreEqual(root.FindSiteTreeNodeByDomainLevel(node => node.DomainLevel == "com"), null);
            Assert.AreEqual(root.FindSiteTreeNodeByDomainLevel(node => node.DomainLevel == "de").DomainLevel, "de");
        }

        [TestMethod]
        public void AddOrGetExistingChild()
        {
            WebsiteTreeNode<String> root = new WebsiteTreeNode<String>(".") { Parent = null };
            WebsiteTreeNode<String> de1 = root.AddOrGetExistingChild("de");
            WebsiteTreeNode<String> de2 = root.AddOrGetExistingChild("de");
            WebsiteTreeNode<String> de3 = root.AddOrGetExistingChild("de");

            Assert.AreEqual(root.Children.Count, 1);
            Assert.AreEqual(de3.IsLeaf, true);
        }
    }
}
