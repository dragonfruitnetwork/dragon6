// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api.Accounts.Enums;
using Realms;

namespace DragonFruit.Six.Client.Database.Entities
{
    [MapTo("saved_accounts")]
    public class SavedAccount : RealmObject
    {
        [PrimaryKey]
        [MapTo("profile_id")]
        public string ProfileId { get; set; }

        [MapTo("ubisoft_id")]
        public string UbisoftId { get; set; }

        [MapTo("username")]
        public string Username { get; set; }

        [Ignored]
        public Platform Platform
        {
            get => (Platform)PlatformValue;
            set => PlatformValue = (int)value;
        }

        [MapTo("platform")]
        private int PlatformValue { get; set; }

        [MapTo("saved_at")]
        public DateTimeOffset SavedAt { get; set; } = DateTimeOffset.Now;

        [MapTo("current_rank")]
        public int CurrentRank { get; set; }
    }
}
