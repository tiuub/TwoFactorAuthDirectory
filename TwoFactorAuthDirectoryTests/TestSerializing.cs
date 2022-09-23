using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoFactorAuthDirectory;

namespace TwoFactorAuthDirectoryTests
{
    [TestClass]
    public class TestSerializing
    {
        [TestMethod]
        public void SerializeDeserialize()
        {
            List<Website> websites = TwoFactorAuthDirectory.TwoFactorAuthDirectory.Deserialize("[[\"(ISC)2\",{\"domain\":\"isc2.org\",\"url\":\"https://www.isc2.org\",\"tfa\":[\"sms\",\"totp\"],\"keywords\":[\"education\"]}]]");

            Assert.AreEqual("[[\"(ISC)2\",{\"Name\":\"(ISC)2\",\"domain\":\"isc2.org\",\"url\":\"https://www.isc2.org\",\"tfa\":[\"totp\",\"sms\"],\"documentation\":\"\",\"recovery\":\"\",\"notes\":\"\",\"contact\":{},\"regions\":[],\"additional-domains\":[],\"custom-software\":[],\"custom-hardware\":[],\"keywords\":[\"education\"],\"img\":\"\"}]]", websites.Serialize());
        }
    }
}
