using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuthDirectory
{
    internal static class CustomConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                WebsiteConverter.Singleton,
                WebsiteMiddlewareConverter.Singleton,
                TfaTypesConverter.Singleton,
            },
        };
    }

    internal class WebsiteConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Website);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                WebsiteMiddleware websiteMiddleware = serializer.Deserialize<WebsiteMiddleware>(reader);
                Website website = websiteMiddleware.Website;
                website.Name = websiteMiddleware.Name;
                return website;

            }
            throw new Exception("Cannot unmarshal type Website");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Website website = value as Website;
            WebsiteMiddleware websiteMiddleware = new WebsiteMiddleware();
            websiteMiddleware.Name = website.Name;
            websiteMiddleware.Website = website;
            serializer.Serialize(writer, websiteMiddleware);
            //throw new NotImplementedException();
        }

        public static readonly WebsiteConverter Singleton = new WebsiteConverter();
    }

    internal class WebsiteMiddlewareConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(WebsiteMiddleware);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray jArray = JArray.Load(reader);
                WebsiteMiddleware websiteMiddleware = new WebsiteMiddleware();
                foreach (JToken jToken in jArray)
                {
                    switch (jToken.Type)
                    {
                        case JTokenType.String:
                        case JTokenType.Date:
                            var stringValue = jToken.ToObject<string>();
                            websiteMiddleware.Name = stringValue;
                            break;
                        case JTokenType.Object:
                            var objectValue = jToken.ToObject<Website>();
                            websiteMiddleware.Website = objectValue;
                            break;
                    }
                }
                return websiteMiddleware;
            }
            throw new Exception("Cannot unmarshal type WebsiteMiddleware");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            WebsiteMiddleware websiteMiddleware = value as WebsiteMiddleware;
            JArray jo = new JArray();
            jo.Add(websiteMiddleware.Name);
            jo.Add(JObject.FromObject(websiteMiddleware.Website));
            jo.WriteTo(writer);
            
            //throw new NotImplementedException();
        }

        public static readonly WebsiteMiddlewareConverter Singleton = new WebsiteMiddlewareConverter();
    }


    internal class TfaTypesConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TfaTypes) || t == typeof(TfaTypes?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            if (reader.TokenType == JsonToken.StartArray)
            {
                TfaTypes tfaTypes = new TfaTypes();

                JArray jArray = JArray.Load(reader);
                foreach (JToken jToken in jArray)
                {
                    if (jToken.Type == JTokenType.String || jToken.Type == JTokenType.Date)
                    {
                        var value = jToken.ToObject<string>();
                        switch (value)
                        {
                            case "call":
                                tfaTypes |= TfaTypes.Call;
                                break;
                            case "custom-hardware":
                                tfaTypes |= TfaTypes.CustomHardware;
                                break;
                            case "custom-software":
                                tfaTypes |= TfaTypes.CustomSoftware;
                                break;
                            case "email":
                                tfaTypes |= TfaTypes.Email;
                                break;
                            case "sms":
                                tfaTypes |= TfaTypes.Sms;
                                break;
                            case "totp":
                                tfaTypes |= TfaTypes.Totp;
                                break;
                            case "u2f":
                                tfaTypes |= TfaTypes.U2F;
                                break;
                            default:
                                tfaTypes |= TfaTypes.Other;
                                break;
                        }
                    }
                }
                return tfaTypes;
            }
            throw new Exception("Cannot unmarshal type TfaTypes");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null) {
                TfaTypes tfaTypes = (TfaTypes)value;
                JArray jArray = new JArray();
                foreach(TfaTypes type in Enum.GetValues(typeof(TfaTypes)))
                {
                    if (tfaTypes.HasFlag(type))
                    {
                        switch (type)
                        {
                            case TfaTypes.Call:
                                jArray.Add("call");
                                break;
                            case TfaTypes.CustomHardware:
                                jArray.Add("custom-hardware");
                                break;
                            case TfaTypes.CustomSoftware:
                                jArray.Add("custom-software");
                                break;
                            case TfaTypes.Email:
                                jArray.Add("email");
                                break;
                            case TfaTypes.Sms:
                                jArray.Add("sms");
                                break;
                            case TfaTypes.Totp:
                                jArray.Add("totp");
                                break;
                            case TfaTypes.U2F:
                                jArray.Add("u2f");
                                break;
                            default:
                                jArray.Add("other");
                                break;
                        }
                    }
                }
                jArray.WriteTo(writer);
            }
        }

        public static readonly TfaTypesConverter Singleton = new TfaTypesConverter();
    }
}
