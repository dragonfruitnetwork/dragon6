// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using Newtonsoft.Json;

namespace DragonFruit.Six.Client.Network.User
{
    public class Dragon6UserCovers : IDragon6UserCoverSources
    {
        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("large")]
        public string Large { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("banner")]
        public string Banner { get; set; }
    }
}
