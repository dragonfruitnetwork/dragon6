// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.IO;
using DragonFruit.Six.Client.Database;

namespace DragonFruit.Six.Client.WebServer
{
    public class WebServerFileSystemStructure : IFileSystemStructure
    {
        public WebServerFileSystemStructure()
        {
            Directory.CreateDirectory(Cache);
            Directory.CreateDirectory(AppData);
        }

        public string Cache => Path.Combine(Path.GetTempPath(), "dragon6");
        public string AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DragonFruit Network", "Dragon6", "Client");
    }
}
