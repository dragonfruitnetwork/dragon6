// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

namespace DragonFruit.Six.Client.Network.User
{
    public interface IDragon6UserCoverSources
    {
        string Raw { get; }

        string Large { get; }
        string Mobile { get; }
        string Banner { get; }
    }
}
