// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace DragonFruit.Six.Client.Network.News
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class UbisoftNewsItemImageInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [CanBeNull]
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
