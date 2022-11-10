// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api.Services.Verification;
using JetBrains.Annotations;
using Realms;

namespace DragonFruit.Six.Client.Database.Entities
{
    [MapTo("cached_users")]
    public class CachedDragon6User : RealmObject
    {
        [MapTo("profile_id")]
        public string ProfileId { get; set; }

        [Ignored]
        public AccountRole Role
        {
            get => (AccountRole)RoleValue;
            set => RoleValue = (byte)value;
        }

        [MapTo("role")]
        private byte RoleValue { get; set; }

        [CanBeNull]
        [MapTo("cover")]
        public string CoverImg { get; set; }

        [CanBeNull]
        [MapTo("title")]
        public string Title { get; set; }

        [CanBeNull]
        [MapTo("title_icon")]
        public string TitleIcon { get; set; }

        [CanBeNull]
        [MapTo("title_colour")]
        public string TitleColour { get; set; }

        [MapTo("expiry")]
        public DateTimeOffset Expires { get; set; } = DateTimeOffset.Now.AddHours(12);
    }
}