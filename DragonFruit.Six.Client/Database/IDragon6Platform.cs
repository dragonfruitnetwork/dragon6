// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

namespace DragonFruit.Six.Client.Database
{
    /// <summary>
    /// Provides a way to set folder structures on a per-system level
    /// </summary>
    public interface IDragon6Platform
    {
        public string Cache { get; }
        public string AppData { get; }

        public void OpenUrl(string url);
    }
}
