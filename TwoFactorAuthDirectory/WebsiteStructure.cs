using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuthDirectory
{
    public class Website
    {
        public String Name { get; set; } = "";

        [JsonProperty(PropertyName = "domain")]
        public String Domain { get; set; } = "";

        [JsonProperty(PropertyName = "url")]
        public String Url { get; set; } = "";

        [JsonProperty(PropertyName = "tfa")]
        [JsonConverter(typeof(TfaTypesConverter))]
        public TfaTypes Tfa { get; set; } = new TfaTypes();

        [JsonProperty(PropertyName = "documentation")]
        public String Documentation { get; set; } = "";

        [JsonProperty(PropertyName = "recovery")]
        public String Recovery { get; set; } = "";

        [JsonProperty(PropertyName = "notes")]
        public String Notes { get; set; } = "";

        [JsonProperty(PropertyName = "contact")]
        public Contact Contact { get; set; } = new Contact();

        [JsonProperty(PropertyName = "regions")]
        public List<String> Regions { get; set; } = new List<String>();

        [JsonProperty(PropertyName = "additional-domains")]
        public List<String> Additional_domains { get; set; } = new List<String>();

        [JsonProperty(PropertyName = "custom-software")]
        public List<String> Custom_software { get; set; } = new List<String>();

        [JsonProperty(PropertyName = "custom-hardware")]
        public List<String> Custom_hardware { get; set; } = new List<String>();

        [JsonProperty(PropertyName = "keywords")]
        public List<String> Keywords { get; set; } = new List<String>();

        [JsonProperty(PropertyName = "img")]
        public String Img { get; set; } = "";

        public bool IsSupporting(TfaTypes tfa)
        {
            return this.Tfa.HasFlag(tfa);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Contact
    {
        [JsonProperty("twitter", NullValueHandling = NullValueHandling.Ignore)]
        public string Twitter { get; set; }

        [JsonProperty("facebook", NullValueHandling = NullValueHandling.Ignore)]
        public string Facebook { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }
    }

    public class WebsiteMiddleware
    {
        public String Name { get; set; }
        public Website Website { get; set; }
    }

    public enum TfaTypes
    {
        Totp = 1 << 0,
        Email = 1 << 1,
        Sms = 1 << 2,
        Call = 1 << 3,
        U2F = 1 << 4,
        CustomHardware = 1 << 5,
        CustomSoftware = 1 << 6,
        Other = 1 << 7,
    }
}
