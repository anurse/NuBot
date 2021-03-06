﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace NuBot.Configuration
{
    public class JsonKeyValueConfiguration : IKeyValueConfiguration
    {
        private Dictionary<string, string> _settings = new Dictionary<string, string>();

        public string GetSetting(string key)
        {
            string ret;
            return _settings.TryGetValue(key, out ret) ? ret : null;
        }

        protected void LoadNewSettings(JProperty settingsProperty)
        {
            var newSettings =
                    settingsProperty != null ?
                        ParseKeyValuePairs(settingsProperty) :
                        new Dictionary<string, string>();
            Interlocked.Exchange(ref _settings, newSettings);
        }

        private static Dictionary<string, string> ParseKeyValuePairs(JProperty settingsProperty)
        {
            if (settingsProperty.Value.Type != JTokenType.Object)
            {
                throw new InvalidDataException("Incorrect Configuration Document");
            }
            var settings = (JObject)settingsProperty.Value;

            return settings.Properties()
                           .ToDictionary(prop => prop.Name, prop => prop.Value.ToString());
        }
    }
}
