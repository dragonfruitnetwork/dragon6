// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DiscordRPC;
using DragonFruit.Six.Client.Configuration;
using DragonFruit.Six.Client.Presence;

// ReSharper disable once CheckNamespace
namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class DiscordPresenceClient : IDisposable
    {
        private const ulong AppId = 679701260177244163;

        private readonly Dragon6Configuration _config;
        private readonly DiscordRpcClient _discord;

        public DiscordPresenceClient(Dragon6Configuration config)
        {
            _config = config;
            _config.SettingUpdated += OnSettingsChanged;

            _discord = new DiscordRpcClient(AppId.ToString())
            {
                SkipIdenticalPresence = true
            };

            if (_config.Get<bool>(Dragon6Setting.EnableDiscordRPC))
            {
                _discord.Initialize();
            }
        }

        public partial void PushUpdate(PresenceStatus status) => _discord.SetPresence(new RichPresence
        {
            State = status.Title,
            Details = status.Subtitle,
            Assets = new Assets
            {
                LargeImageKey = "dragon6-cover",
                LargeImageText = "Dragon6"
            }
        });

        private void OnSettingsChanged(Dragon6Setting setting, object value)
        {
            if (setting != Dragon6Setting.EnableDiscordRPC)
            {
                return;
            }

            var enabled = (bool)value;

            if (enabled == _discord.IsInitialized)
            {
                return;
            }

            if (_discord.IsInitialized)
            {
                _discord.Deinitialize();
            }
            else
            {
                _discord.Initialize();
            }
        }

        public void Dispose()
        {
            _config.SettingUpdated -= OnSettingsChanged;
            _discord?.Dispose();
        }
    }
}
