// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;

namespace DragonFruit.Six.Client.Overlays.Status
{
    public class ServerStatusButtonInfo : ICustomButton
    {
        public int Order => 4;
        public int ColumnSize => 4;

        public Type ButtonComponentType => typeof(ServerStatusButton);
    }
}
