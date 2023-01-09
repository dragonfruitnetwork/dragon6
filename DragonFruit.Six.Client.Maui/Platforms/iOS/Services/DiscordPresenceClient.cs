// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

// ReSharper disable CheckNamespace

using System;
using DragonFruit.Six.Client.Presence;

namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class DiscordPresenceClient
    {
        public partial void PushUpdate(PresenceStatus status) => throw new PlatformNotSupportedException();
    }
}
