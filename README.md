# TwoFactorAuthDirectory (related to 2fa.directory)
[![NuGet Releases](https://img.shields.io/nuget/v/TwoFactorAuthDirectory)](https://www.nuget.org/packages/TwoFactorAuthDirectory/)
[![NuGet Releases](https://img.shields.io/nuget/dt/TwoFactorAuthDirectory)](https://www.nuget.org/packages/TwoFactorAuthDirectory/)
[![Issues](https://img.shields.io/github/issues/tiuub/TwoFactorAuthDirectory)](https://github.com/tiuub/TwoFactorAuthDirectory/issues)
[![GitHub](https://img.shields.io/github/license/tiuub/TwoFactorAuthDirectory)](https://github.com/tiuub/TwoFactorAuthDirectory/blob/master/LICENSE.md)
[![Donate](https://img.shields.io/badge/donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=5F5QB7744AD5G&source=url)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/tiuub)](https://github.com/sponsors/tiuub)


KeeOtp2 is a simple NuGet package to determine which website supports which type of 2fa. Therefore it uses the [twofactorauth](https://github.com/2factorauth/twofactorauth) API, which is provided by GitHub user [2factorauth](https://github.com/2factorauth). *(They also have a frontend application, which you can find [here](https://2fa.directory/).)*


## Installation
This guide is for [Visual Studio](https://visualstudio.microsoft.com/de/) only
 - Right-click on your project in the Solution Explorer
 - Select **Manage NuGet Packages**
 - Click Browse and search for **TwoFactorAuthDirectory**
 - Select the one **by tiuub** and install it


## Usage

### Fetching websites from API

```c#
...

TwoFactorAuthClient client = new TwoFactorAuthClient();

// Setting a custom API url (optional)
client.ApiUrl = "https://twofactorauth.tiuub.de/frozen/2022-07-06/api/v3/all.json";

// Fetching websites from API asynchronously
List<Website> websites = client.FetchAsync().Result;

// Fetching websites from API synchronously
List<Website> websites = client.Fetch();

...
```



## Sorting

```c#
...

TwoFactorAuthClient client = new TwoFactorAuthClient();
List<Website> websites = client.Fetch();

// Find all websites which include "Google" in their name
List<Website> websitesByName = websites.FindByName("Google", ignoreCase: true);

// Find all websites which have the exact domain "mail.google.com"
List<Website> websitesByDomain = websites.FindByDomain("mail.google.com");

// Find all websites which have parts of the given url in their url field or in their additional_domains/domain field
List<Website> websitesByUrl = websites.FindByUrl("https://log:mein@samsung.com/test-path?test-parameter:123");

// Find all websites which supports a specific type of tfa
List<Website> websitesByTfa = websites.FindByTfa(TfaTypes.Totp | TfaTypes.Sms);

// Find all websites which are subjected by specific keywords
List<Website> websitesByKeywords = websites.FindByKeywords(new List<String>() { "email", "security" });

// Find all websites which are subjected by specific regions
List<Website> websitesByRegions = websites.FindByRegions(new List<String>() { "us", "de" });

...
```


## License

[![GitHub](https://img.shields.io/github/license/tiuub/TwoFactorAuthDirectory)](https://github.com/tiuub/TwoFactorAuthDirectory/blob/master/LICENSE.md)



## Dependencies

Dependencie | Source | NuGet | Author | License
--- | --- | --- | --- | ---
**Newtonsoft.Json** | [source](https://github.com/JamesNK/Newtonsoft.Json) | [NuGet](https://www.nuget.org/packages/Newtonsoft.Json) | [newtonsoft](https://www.nuget.org/profiles/newtonsoft) | [MIT](https://licenses.nuget.org/MIT)



### Dependencies (TwoFactorAuthDirectoryTests)

Dependencie | Source | NuGet | Author | License
--- | --- | --- | --- | ---
**coverlet.collector** | [source](https://github.com/coverlet-coverage/coverlet) | [NuGet](https://www.nuget.org/packages/coverlet.collector/) | [tonerdo](https://www.nuget.org/profiles/tonerdo) | [MIT](https://licenses.nuget.org/MIT)
**Microsoft.NET.Test.Sdk** | [source](https://github.com/microsoft/vstest/) | [NuGet](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/) | [Microsoft](https://www.nuget.org/profiles/Microsoft) | [MIT](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/17.3.1/License)
**MSTest.TestAdapter** | [source](https://github.com/microsoft/testfx) | [NuGet](https://www.nuget.org/packages/MSTest.TestAdapter/) | [Microsoft](https://www.nuget.org/profiles/Microsoft) | [MIT](https://licenses.nuget.org/MIT)
**MSTest.TestFramework** | [source](https://github.com/microsoft/testfx) | [NuGet](https://www.nuget.org/packages/MSTest.TestFramework/) | [Microsoft](https://www.nuget.org/profiles/Microsoft) | [MIT](https://licenses.nuget.org/MIT)
