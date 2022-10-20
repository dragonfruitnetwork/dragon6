// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Api.Accounts.Entities;

namespace DragonFruit.Six.Client.Overlays.Search
{
    /// <summary>
    /// App-wide state for searching for users
    /// </summary>
    public class SearchProviderState
    {
        /// <summary>
        /// The most recently discovered account
        /// </summary>
        public UbisoftAccount DiscoveredAccount { get; set; }
    }
}
