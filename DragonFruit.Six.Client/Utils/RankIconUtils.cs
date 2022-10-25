// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

namespace DragonFruit.Six.Client.Utils
{
    public static class RankIconUtils
    {
        public static string FormatRankIconUrl(string url)
        {
            return "https://d6static.dragonfruit.network/" + url.TrimStart('/').Replace("-png", string.Empty).Replace(".png", ".svg");
        }
    }
}
