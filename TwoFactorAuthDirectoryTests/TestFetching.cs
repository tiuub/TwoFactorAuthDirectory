using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoFactorAuthDirectory;

namespace TwoFactorAuthDirectoryTests
{
    [TestClass]
    public class TestFetching
    {
        [TestMethod]
        public void FetchAsync()
        {
            TwoFactorAuthDirectory.TwoFactorAuthClient client = new TwoFactorAuthDirectory.TwoFactorAuthClient();
            client.TwoFactorAuthApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            client.fetchAsync().Wait();

            Assert.AreEqual(false, client.Websites.IsLeaf);
            Assert.AreEqual(2701, client.Websites.Count);
            Assert.AreEqual(6, client.Websites.Depth);
        }

        [TestMethod]
        public void Find()
        {
            TwoFactorAuthDirectory.TwoFactorAuthClient client = new TwoFactorAuthDirectory.TwoFactorAuthClient();
            client.TwoFactorAuthApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            client.fetchAsync().Wait();

            List<Website> websites = client.find(website => website.Domain == "mail.google.com");
            Assert.AreEqual(1, websites.Count);
            Assert.AreEqual("Gmail", websites[0].Name);

            websites = client.find(website => website.Name.Contains("Samsung"));
            Assert.AreEqual(2, websites.Count);
            Assert.AreEqual("Samsung SmartThings", websites[1].Name);

            websites = client.find(website => website.IsSupporting(TfaTypes.Totp | TfaTypes.Sms));
            Assert.AreEqual(343, websites.Count);
            Assert.AreEqual("(ISC)2", websites[0].Name);
        }

        [TestMethod]
        public void FindByUrl()
        {
            TwoFactorAuthDirectory.TwoFactorAuthClient client = new TwoFactorAuthDirectory.TwoFactorAuthClient();
            client.TwoFactorAuthApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            client.fetchAsync().Wait();

            List<Website> websites = client.findAllWebsitesByUrl("mail.google.com");
            Assert.AreEqual(1, websites.Count);
            Assert.AreEqual("Gmail", websites[0].Name);

            websites = client.findAllWebsitesByUrl("https://log:mein@samsung.com/test-path?test-parameter:123");
            Assert.AreEqual(1, websites.Count);
            Assert.AreEqual("Samsung", websites[0].Name);
        }

        [TestMethod]
        public void FindByTfa()
        {
            TwoFactorAuthDirectory.TwoFactorAuthClient client = new TwoFactorAuthDirectory.TwoFactorAuthClient();
            client.TwoFactorAuthApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            client.fetchAsync().Wait();

            List<Website> websites = client.findAllWebsitesByTfa(TfaTypes.Totp | TfaTypes.Sms | TfaTypes.CustomSoftware);
            Assert.AreEqual(58, websites.Count);
            Assert.AreEqual("NuGet", websites[0].Name);

            websites = client.findAllWebsitesByTfa(TfaTypes.Totp | TfaTypes.Sms | TfaTypes.CustomSoftware | TfaTypes.Email);
            Assert.AreEqual(18, websites.Count);
            Assert.AreEqual("Bitwarden", websites[0].Name);
        }

        [TestMethod]
        public void FindByKeyword()
        {
            TwoFactorAuthDirectory.TwoFactorAuthClient client = new TwoFactorAuthDirectory.TwoFactorAuthClient();
            client.TwoFactorAuthApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            client.fetchAsync().Wait();

            List<Website> websites = client.findAllWebsitesByKeyword("education");
            Assert.AreEqual(47, websites.Count);

            websites = client.findAllWebsitesByKeyword("banking");
            Assert.AreEqual(230, websites.Count);
        }

        [TestMethod]
        public void TFA()
        {
            TwoFactorAuthDirectory.TwoFactorAuthClient client = new TwoFactorAuthDirectory.TwoFactorAuthClient();
            client.TwoFactorAuthApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            client.fetchAsync().Wait();

            List<Website> websites = client.findAllWebsitesByUrl("mail.google.com");
            Assert.AreEqual(1, websites.Count);
            Assert.AreEqual(true, websites[0].IsSupporting(TfaTypes.Totp | TfaTypes.Sms | TfaTypes.Call | TfaTypes.CustomSoftware | TfaTypes.U2F));
            websites = client.findAllWebsitesByUrl("https://log:mein@samsung.com/test-path?test-parameter:123");
            Assert.AreEqual(1, websites.Count);
            Assert.AreEqual(true, websites[0].IsSupporting(TfaTypes.Totp | TfaTypes.Sms));
        }
    }
}
