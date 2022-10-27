// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api.Accounts.Enums;

namespace DragonFruit.Six.Client.Utils
{
    public static class PlatformUtils
    {
        public static (string icon, string colour) GetPlatformIcon(this Platform platform) => platform switch
        {
            Platform.PC => ("windows", "#00ADEF"),
            Platform.PSN => ("playstation", "#4C87C7"),
            Platform.XB1 => ("xbox", "#107C0F"),

            _ => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
        };
    }
}
