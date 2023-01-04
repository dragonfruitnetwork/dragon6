// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

namespace DragonFruit.Six.Client.Presence
{
    public interface IPresenceClient
    {
        void PushUpdate(PresenceStatus status);
    }
}
