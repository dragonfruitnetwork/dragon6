// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DragonFruit.Six.Client.Database;

namespace DragonFruit.Six.Client.Configuration
{
    public class Dragon6Configuration
    {
        private const string FileName = "config.ini";

        private int _changeCount;
        private readonly SharpConfig.Configuration _configuration;
        private readonly IDragon6Platform _platform;

        public Dragon6Configuration(IDragon6Platform platform)
        {
            _platform = platform;
            _configuration = SharpConfig.Configuration.LoadFromFile(Path.Combine(_platform.AppData, FileName));

            Task.Run(SetDefaults);
        }

        /// <summary>
        /// Gets a <see cref="Dragon6Setting"/> from the configuration container
        /// </summary>
        /// <param name="setting">The setting to retrieve</param>
        /// <typeparam name="T">The type the value represents</typeparam>
        public T Get<T>(Dragon6Setting setting) where T : notnull, new()
        {
            var settingName = setting.ToString();
            return _configuration.DefaultSection[settingName].GetValue<T>();
        }

        /// <summary>
        /// Sets a <see cref="Dragon6Setting"/> value, queueing the configuration to be saved when modification ends
        /// </summary>
        /// <param name="setting">The <see cref="Dragon6Setting"/> to set</param>
        /// <param name="value">The value to apply to the setting</param>
        public void Set(Dragon6Setting setting, object value)
        {
            _configuration[setting.ToString()].SetValuesTo(value);
            var changeCount = Interlocked.Increment(ref _changeCount);

            Task.Delay(100).ContinueWith(_ =>
            {
                if (changeCount == _changeCount)
                {
                    _configuration.SaveToFile(Path.Combine(_platform.AppData, FileName));
                }
            });
        }

        private void SetDefaults()
        {
            foreach (var setting in Enum.GetValues<Dragon6Setting>())
            {
                var settingName = setting.ToString();

                if (!_configuration.DefaultSection.Contains(settingName))
                {
                    Set(setting, typeof(Dragon6Setting).GetField(settingName).GetCustomAttribute<DefaultValueAttribute>()?.Value);
                }
            }
        }
    }
}
