// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Accounts.Enums;

namespace DragonFruit.Six.Client.Overlays.Search
{
    /// <summary>
    /// App-wide state for searching for users
    /// </summary>
    public class SearchProviderState
    {
        public event Action<string, Platform, IdentifierType?> AccountSearchRequested;

        /// <summary>
        /// The most recently discovered account
        /// </summary>
        public UbisoftAccount DiscoveredAccount { get; set; }

        /// <summary>
        /// Triggers an account search
        /// </summary>
        public void TriggerSearch(string identifier, Platform platform, IdentifierType? type = null)
        {
            AccountSearchRequested?.Invoke(identifier, platform, type);
        }
    }
}
