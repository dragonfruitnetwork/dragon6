// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts.Enums;

namespace DragonFruit.Six.Client.Database
{
    public static class PlatformUtils
    {
        /// <summary>
        /// Converts a ubisoft platform name to a <see cref="Platform"/>
        /// </summary>
        public static Platform GetPlatform(string platform) => platform switch
        {
            UbisoftPlatforms.PC => Platform.PC,
            UbisoftPlatforms.XBOX => Platform.XB1,
            UbisoftPlatforms.PLAYSTATION => Platform.PSN,
            UbisoftPlatforms.CROSSPLAY => Platform.CrossPlatform,

            _ => throw new ArgumentOutOfRangeException()
        };

        /// <summary>
        /// Converts a <see cref="Platform"/> to its ubisoft-recognised name
        /// </summary>
        public static string ToPlatformName(this Platform platform) => platform switch
        {
            Platform.PC => UbisoftPlatforms.PC,
            Platform.XB1 => UbisoftPlatforms.XBOX,
            Platform.PSN => UbisoftPlatforms.PLAYSTATION,
            Platform.CrossPlatform => UbisoftPlatforms.CROSSPLAY,

            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
