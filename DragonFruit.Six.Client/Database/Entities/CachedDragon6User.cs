// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Client.Network.User;
using JetBrains.Annotations;
using Realms;

namespace DragonFruit.Six.Client.Database.Entities
{
    [MapTo("cached_users")]
    public class CachedDragon6User : RealmObject, IDragon6User
    {
        [PrimaryKey]
        [MapTo("ubisoft_id")]
        public string UbisoftId { get; set; }

        [Ignored]
        public AccountRole AccountRole
        {
            get => (AccountRole)RoleValue;
            set => RoleValue = (int)value;
        }

        [MapTo("role")]
        private int RoleValue { get; set; }

        [CanBeNull]
        [MapTo("cover")]
        public CachedDragon6UserCoverSources Covers { get; set; }

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

        [Ignored]
        IDragon6UserCoverSources IDragon6User.Covers => Covers;
    }
}
