// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api.Services.Verification;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Realms;

namespace DragonFruit.Six.Client.Network.User
{
    [Serializable]
    [MapTo("cached_users")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Dragon6User : RealmObject
    {
        [MapTo("profile_id")]
        [JsonProperty("profile_id")]
        public string ProfileId { get; set; }

        [Ignored]
        public AccountRole Role
        {
            get => (AccountRole)RoleValue;
            set => RoleValue = (byte)value;
        }

        [MapTo("role")]
        [JsonProperty("role")]
        private byte RoleValue { get; set; }

        [CanBeNull]
        [MapTo("cover")]
        [JsonProperty("cover_img")]
        public string CoverImg { get; set; }

        [CanBeNull]
        [MapTo("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [CanBeNull]
        [MapTo("title_icon")]
        [JsonProperty("title_icon")]
        public string TitleIcon { get; set; }

        [CanBeNull]
        [MapTo("title_colour")]
        [JsonProperty("title_colour")]
        public string TitleColour { get; set; }

        [MapTo("expiry")]
        public DateTimeOffset Expires { get; set; }
    }
}
