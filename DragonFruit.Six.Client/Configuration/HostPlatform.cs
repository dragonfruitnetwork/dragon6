// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Diagnostics.CodeAnalysis;

namespace DragonFruit.Six.Client.Configuration
{
    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum HostPlatform
    {
        Windows = 1,
        Linux = 2,
        Mac = 4,
        iOS = 8,
        Android = 16,

        Mobile = iOS | Android,
        Desktop = Windows | Linux | Mac
    }
}
