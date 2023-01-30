// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Client.Database.Entities;

namespace DragonFruit.Six.Client.Network.User
{
    public interface IDragon6User
    {
        string UbisoftId { get; }
        AccountRole AccountRole { get; }

        IDragon6UserCoverSources Covers { get; }

        string Title { get; }
        string TitleIcon { get; }
        string TitleColour { get; }
    }
}
