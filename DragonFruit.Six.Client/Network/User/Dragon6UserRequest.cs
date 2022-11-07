// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Data;

namespace DragonFruit.Six.Client.Network.User
{
    public class Dragon6UserRequest : ApiRequest
    {
        public override string Path => $"https://dragon6.dragonfruit.network/api/v2/users/{ProfileId}";

        public Dragon6UserRequest(string profileId)
        {
            ProfileId = profileId;
        }

        public string ProfileId { get; }
    }
}
