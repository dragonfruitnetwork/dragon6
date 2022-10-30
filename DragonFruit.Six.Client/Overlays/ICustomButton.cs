// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;

namespace DragonFruit.Six.Client.Overlays
{
    /// <summary>
    /// An advanced button that has it's own implementation type
    /// </summary>
    public interface ICustomButton : IHeaderButton
    {
        Type ButtonComponentType { get; }
    }
}
