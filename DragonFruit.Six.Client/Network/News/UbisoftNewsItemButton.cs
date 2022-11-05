// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using Newtonsoft.Json;

namespace DragonFruit.Six.Client.Network.News
{
    public class UbisoftNewsItemButton
    {
        [JsonProperty("buttonUrl")]
        public string Stub { get; set; }
    }
}
