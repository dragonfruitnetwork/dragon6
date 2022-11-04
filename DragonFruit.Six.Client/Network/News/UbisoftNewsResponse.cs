// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DragonFruit.Six.Client.Network.News
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class UbisoftNewsResponse
    {
        [JsonProperty("skip")]
        public int Skip { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("tags")]
        public IReadOnlyList<string> Tags { get; set; }

        [JsonProperty("items")]
        public IReadOnlyCollection<UbisoftNewsItem> Items { get; set; }
    }
}
