// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

namespace DragonFruit.Six.Client.Overlays
{
    /// <summary>
    /// Represents the base of any Dragon6 header button
    /// </summary>
    public interface IHeaderButton
    {
        int Order { get; }

        int ColumnSize { get; }
    }
}
