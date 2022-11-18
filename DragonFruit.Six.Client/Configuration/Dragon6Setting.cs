// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.ComponentModel;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Api.Seasonal.Enums;

namespace DragonFruit.Six.Client.Configuration
{
    public enum Dragon6Setting
    {
        [DefaultValue(Region.EMEA)]
        LegacyStatsRegion,

        [DefaultValue(Platform.PC)]
        [Description("Default Platform")]
        DefaultPlatform,

        [DefaultValue(BoardType.Ranked)]
        [Description("Default Seasonal Stats Type")]
        DefaultSeasonalType,

        // ReSharper disable once InconsistentNaming
        [DefaultValue(true)]
        [Description("Enable Discord Status")]
        [Dragon6PlatformSpecificSetting(HostPlatform.Desktop)]
        EnableDiscordRPC
    }
}
