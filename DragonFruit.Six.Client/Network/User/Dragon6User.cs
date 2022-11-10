// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api.Services.Verification;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace DragonFruit.Six.Client.Network.User
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class Dragon6User
    {
        [JsonProperty("profile_id")]
        public string ProfileId { get; set; }

        [JsonProperty("role")]
        public AccountRole Role { get; set; }

        [CanBeNull]
        [JsonProperty("cover_img")]
        public string CoverImg { get; set; }

        [CanBeNull]
        [JsonProperty("title")]
        public string Title { get; set; }

        [CanBeNull]
        [JsonProperty("title_icon")]
        public string TitleIcon { get; set; }

        [CanBeNull]
        [JsonProperty("title_colour")]
        public string TitleColour { get; set; }
    }
}
