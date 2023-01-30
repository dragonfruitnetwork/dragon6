// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Client.Database.Entities;
using Newtonsoft.Json;

namespace DragonFruit.Six.Client.Network.User
{
    public class Dragon6User : IDragon6User
    {
        [JsonProperty("ubisoft_id")]
        public string UbisoftId { get; set; }

        [JsonProperty("profile_type")]
        public AccountRole AccountRole { get; set; }

        [JsonProperty("cover")]
        private Dragon6UserCovers Covers { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_icon")]
        public string TitleIcon { get; set; }

        [JsonProperty("title_colour")]
        public string TitleColour { get; set; }

        [JsonIgnore]
        IDragon6UserCoverSources IDragon6User.Covers => Covers;
    }
}
