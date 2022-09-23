using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoFactorAuthDirectory;

namespace TwoFactorAuthDirectoryTests
{
    [TestClass]
    public class TestFind
    {
        [TestMethod]
        public void Find()
        {
            TwoFactorAuthClient client = new TwoFactorAuthClient();
            client.ApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            List<Website> websites = client.Fetch();


            List<Website> tmpWebsites = websites.FindAll(website => website.Domain == "mail.google.com");
            Assert.AreEqual(1, tmpWebsites.Count);
            Assert.AreEqual("Gmail", tmpWebsites[0].Name);


            tmpWebsites = websites.FindAll(website => website.Name.Contains("Samsung"));
            Assert.AreEqual(2, tmpWebsites.Count);
            Assert.AreEqual("Samsung SmartThings", tmpWebsites[1].Name);

            tmpWebsites = websites.FindAll(website => website.IsSupporting(TfaTypes.Totp | TfaTypes.Sms));
            Assert.AreEqual(343, tmpWebsites.Count);
            Assert.AreEqual("(ISC)2", tmpWebsites[0].Name);
        }

        [TestMethod]
        public void FindByUrl()
        {
            TwoFactorAuthClient client = new TwoFactorAuthClient();
            client.ApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            List<Website> websites = client.Fetch();

            List<Website> tmpWebsites = websites.FindByUrl("mail.google.com");
            Assert.AreEqual(1, tmpWebsites.Count);
            Assert.AreEqual("Gmail", tmpWebsites[0].Name);

            tmpWebsites = websites.FindByUrl("https://log:mein@samsung.com/test-path?test-parameter:123");
            Assert.AreEqual(1, tmpWebsites.Count);
            Assert.AreEqual("Samsung", tmpWebsites[0].Name);
        }

        [TestMethod]
        public void FindByTfa()
        {
            TwoFactorAuthClient client = new TwoFactorAuthClient();
            client.ApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            List<Website> websites = client.Fetch();

            List<Website> tmpWebsites = websites.FindByTfa(TfaTypes.Totp | TfaTypes.Sms | TfaTypes.CustomSoftware);
            Assert.AreEqual(58, tmpWebsites.Count);

            tmpWebsites = websites.FindByTfa(TfaTypes.Totp | TfaTypes.Sms | TfaTypes.CustomSoftware | TfaTypes.Email);
            Assert.AreEqual(18, tmpWebsites.Count);
        }

        [TestMethod]
        public void FindByKeywords()
        {
            TwoFactorAuthClient client = new TwoFactorAuthClient();
            client.ApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            List<Website> websites = client.Fetch();

            List<Website> tmpWebsites = websites.FindByKeywords(new List<String>() { "email", "security" });
            Assert.AreEqual(1, tmpWebsites.Count);

            tmpWebsites = websites.FindByKeywords(new List<String>() { "education" });
            Assert.AreEqual(47, tmpWebsites.Count);

            tmpWebsites = websites.FindByKeywords(new List<String>() { "banking" });
            Assert.AreEqual(230, tmpWebsites.Count);
        }

        [TestMethod]
        public void FindByRegions()
        {
            TwoFactorAuthClient client = new TwoFactorAuthClient();
            client.ApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            List<Website> websites = client.Fetch();

            List<Website> tmpWebsites = websites.FindByRegions(new List<string>() {"de"});
            Assert.AreEqual(111, tmpWebsites.Count);

            tmpWebsites = websites.FindByRegions(new List<string>() {"us", "de"});
            Assert.AreEqual(19, tmpWebsites.Count);
        }
    }
}
