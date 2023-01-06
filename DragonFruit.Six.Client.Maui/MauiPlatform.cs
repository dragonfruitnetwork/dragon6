// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.IO;
using DragonFruit.Six.Client.Configuration;
using DragonFruit.Six.Client.Database;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;
using Browser = Microsoft.Maui.ApplicationModel.Browser;

namespace DragonFruit.Six.Client.Maui
{
    public class MauiPlatform : IDragon6Platform
    {
        public MauiPlatform()
        {
            Directory.CreateDirectory(Cache);
            Directory.CreateDirectory(AppData);
        }

        public string Cache => FileSystem.CacheDirectory;

        public string AppData => FileSystem.AppDataDirectory;

        public HostPlatform CurrentPlatform
        {
            get
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
                {
                    return HostPlatform.Windows;
                }

                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    return HostPlatform.Android;
                }

                if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                {
                    return HostPlatform.iOS;
                }

                if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst || DeviceInfo.Current.Platform == DevicePlatform.macOS)
                {
                    return HostPlatform.Mac;
                }

                return HostPlatform.Desktop;
            }
        }

        public void OpenUrl(string url)
        {
            Browser.Default.OpenAsync(url, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                PreferredToolbarColor = new Color(0xBD, 0x18, 0x18)
            });
        }
    }
}
