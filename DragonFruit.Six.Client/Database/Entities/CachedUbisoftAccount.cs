// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Accounts.Enums;
using Realms;

namespace DragonFruit.Six.Client.Database.Entities
{
    public class CachedUbisoftAccount : RealmObject, IEquatable<UbisoftAccount>
    {
        [PrimaryKey]
        [MapTo("profile_id")]
        public string ProfileId { get; set; }

        [MapTo("ubisoft_id")]
        public string UbisoftId { get; set; }

        [MapTo("platform_id")]
        public string PlatformId { get; set; }

        [MapTo("username")]
        public string Username { get; set; }

        [Ignored]
        public Platform Platform
        {
            get => (Platform)PlatformValue;
            set => PlatformValue = (int)value;
        }

        [MapTo("platform")]
        internal int PlatformValue { get; set; }

        [MapTo("expiry")]
        public DateTimeOffset Expires { get; set; } = DateTimeOffset.Now.AddHours(6);

        public bool Equals(UbisoftAccount other) => ProfileId == other?.ProfileId;

        public override string ToString() => $"{Username} ({UbisoftId} - {Platform})";
    }
}
