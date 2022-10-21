// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Client.Database;
using Microsoft.Maui.Storage;

namespace DragonFruit.Six.Client.Maui
{
    public class MauiFileSystemStructure : IFileSystemStructure
    {
        public string Cache => FileSystem.CacheDirectory;
        public string AppData => FileSystem.AppDataDirectory;
    }
}
