// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Diagnostics;
using System.IO;
using DragonFruit.Six.Client.Configuration;
using DragonFruit.Six.Client.Database;

namespace DragonFruit.Six.Client.WebServer
{
    public class WebServerPlatform : IDragon6Platform
    {
        public WebServerPlatform()
        {
            Directory.CreateDirectory(Cache);
            Directory.CreateDirectory(AppData);
        }

        public string Cache => Path.Combine(AppData, "cache");
        public string AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DragonFruit Network", "Dragon6", "Client");

        public HostPlatform CurrentPlatform => HostPlatform.Desktop;

        public void OpenUrl(string url)
        {
            var psi = new ProcessStartInfo(url)
            {
                Verb = "open",
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}
