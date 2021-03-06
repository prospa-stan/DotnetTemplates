﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ProspaAspNetCoreApiNsb.Infrastructure
{
    public static class DefaultMessageJsonSerializerSettings
    {
        public const string ContentType = "application/json";
        private const int DefaultMaxDepth = 32;

        private static readonly DefaultContractResolver DefaultContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        private static readonly Lazy<JsonSerializerSettings> Lazy =
            new Lazy<JsonSerializerSettings>(
                () =>
                {
                    var settings = new JsonSerializerSettings
                    {
                        DateParseHandling = DateParseHandling.DateTimeOffset,
                        MaxDepth = DefaultMaxDepth,
                        ContractResolver = DefaultContractResolver,
                        Formatting = Formatting.Indented,
                        TypeNameHandling = TypeNameHandling.None
                    };

                    settings.Converters.Add(new StringEnumConverter());

                    return settings;
                });

        public static JsonSerializerSettings Instance => Lazy.Value;
    }
}
