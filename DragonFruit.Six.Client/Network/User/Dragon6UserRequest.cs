// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Collections.Generic;
using DragonFruit.Data;
using DragonFruit.Data.Parameters;

namespace DragonFruit.Six.Client.Network.User
{
    public class Dragon6UserRequest : ApiRequest
    {
        public override string Path => "https://dragon6.dragonfruit.network/api/v3/users";

        public Dragon6UserRequest(IEnumerable<string> profileIds)
        {
            ProfileIds = profileIds;
        }

        [QueryParameter("ids", CollectionConversionMode.Recursive)]
        public IEnumerable<string> ProfileIds { get; }
    }
}
