// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DragonFruit.Six.Client.Database;
using Microsoft.Extensions.Logging;
using SharpConfig;

namespace DragonFruit.Six.Client.Configuration
{
    public class Dragon6Configuration
    {
        private const string FileName = "config.ini";
        private const string Category = "Dragon6";

        private int _changeCount;

        private readonly string _storagePath;
        private readonly ILogger<Dragon6Configuration> _logger;
        private readonly SharpConfig.Configuration _configuration;

        public Dragon6Configuration(IDragon6Platform platform, ILogger<Dragon6Configuration> logger)
        {
            _logger = logger;
            _storagePath = Path.Combine(platform.AppData, FileName);
            _configuration = File.Exists(_storagePath) ? SharpConfig.Configuration.LoadFromFile(_storagePath) : new SharpConfig.Configuration();
        }

        /// <summary>
        /// Event fired when a <see cref="Dragon6Setting"/> has been changed
        /// </summary>
        public event Action<Dragon6Setting, object> SettingUpdated;

        /// <summary>
        /// Gets a <see cref="Dragon6Setting"/> from the configuration container
        /// </summary>
        /// <param name="setting">The setting to retrieve</param>
        /// <typeparam name="T">The type the value represents</typeparam>
        public T Get<T>(Dragon6Setting setting) where T : notnull, new()
        {
            try
            {
                var settingName = setting.ToString();
                return _configuration[Category][settingName].GetValue<T>();
            }
            catch (SettingValueCastException)
            {
                return default;
            }
        }

        /// <summary>
        /// Sets a <see cref="Dragon6Setting"/> value, queueing the configuration to be saved when modification ends
        /// </summary>
        /// <param name="setting">The <see cref="Dragon6Setting"/> to set</param>
        /// <param name="value">The value to apply to the setting</param>
        public void Set<T>(Dragon6Setting setting, T value)
        {
            SettingUpdated?.Invoke(setting, value);

            _configuration[Category][setting.ToString()].SetValue(value);
            _logger.LogInformation("{setting} changed to {value}", setting, value);

            var changeCount = Interlocked.Increment(ref _changeCount);

            Task.Delay(500).ContinueWith(_ =>
            {
                if (changeCount == _changeCount)
                {
                    _configuration.SaveToFile(_storagePath);
                }
            });
        }

        internal void SetDefaults()
        {
            foreach (var setting in Enum.GetValues<Dragon6Setting>())
            {
                var settingName = setting.ToString();

                if (!_configuration[Category].Contains(settingName))
                {
                    Set(setting, typeof(Dragon6Setting).GetField(settingName).GetCustomAttribute<DefaultValueAttribute>()?.Value);
                }
            }
        }
    }
}
