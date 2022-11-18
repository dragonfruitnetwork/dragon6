// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;

namespace DragonFruit.Six.Client.Configuration
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Dragon6PlatformSpecificSetting : Attribute
    {
        public Dragon6PlatformSpecificSetting(HostPlatform supportedPlatforms)
        {
            SupportedPlatforms = supportedPlatforms;
        }

        public HostPlatform SupportedPlatforms { get; }
    }
}
