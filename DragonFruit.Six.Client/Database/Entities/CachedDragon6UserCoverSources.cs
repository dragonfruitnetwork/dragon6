﻿// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Client.Network.User;
using Realms;

namespace DragonFruit.Six.Client.Database.Entities
{
    [MapTo("cached_user_covers")]
    public class CachedDragon6UserCoverSources : EmbeddedObject, IDragon6UserCoverSources
    {
        [MapTo("raw")]
        public string Raw { get; set; }

        [MapTo("large")]
        public string Large { get; set; }

        [MapTo("mobile")]
        public string Mobile { get; set; }

        [MapTo("banner")]
        public string Banner { get; set; }
    }
}
