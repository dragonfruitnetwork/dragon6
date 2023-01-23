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
        private AccountSearchArgs _lastUnhandledSearchArgs;

        /// <summary>
        /// Event fired when an account search is requested.
        /// </summary>
        public event Action<AccountSearchArgs> AccountSearchRequested;

        /// <summary>
        /// The most recently discovered account
        /// </summary>
        public UbisoftAccount DiscoveredAccount { get; set; }

        /// <summary>
        /// The last unhandled <see cref="AccountSearchArgs"/> requested by the system.
        /// </summary>
        public AccountSearchArgs LastUnhandledSearch
        {
            get
            {
                var args = _lastUnhandledSearchArgs;
                _lastUnhandledSearchArgs = null;

                return args;
            }
            private set => _lastUnhandledSearchArgs = value;
        }

        /// <summary>
        /// Triggers an account search using the provided criteria
        /// </summary>
        /// <param name="identifier">The username or ubisoft id of the user to load</param>
        /// <param name="platform">The <see cref="Platform"/> the user is playing on</param>
        /// <param name="type">Optional <see cref="IdentifierType"/> if known</param>
        public void TriggerSearch(string identifier, Platform platform, IdentifierType? type = null)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return;
            }

            TriggerSearch(new AccountSearchArgs(identifier, platform, type));
        }

        /// <summary>
        /// Triggers an account search using the provided criteria
        /// </summary>
        /// <param name="args">The <see cref="AccountSearchArgs"/> query to request</param>
        public void TriggerSearch(AccountSearchArgs args)
        {
            if (AccountSearchRequested?.GetInvocationList().Length > 0)
            {
                AccountSearchRequested.Invoke(args);
                return;
            }

            LastUnhandledSearch = args;
        }
    }
}
