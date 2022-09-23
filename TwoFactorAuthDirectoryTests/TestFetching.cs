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
        public void FetchSync()
        {
            TwoFactorAuthClient client = new TwoFactorAuthClient();
            client.ApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            List<Website> websites = client.Fetch();

            Assert.AreEqual(2063, websites.Count);
        }

        [TestMethod]
        public void FetchAsync()
        {
            TwoFactorAuthClient client = new TwoFactorAuthClient();
            client.ApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";
            List<Website> websites = client.FetchAsync().Result;

            Assert.AreEqual(2063, websites.Count);
        }
    }
}
