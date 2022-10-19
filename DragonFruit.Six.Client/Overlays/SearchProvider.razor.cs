// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Threading.Tasks;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Accounts.Enums;
using Havit.Blazor.Components.Web.Bootstrap;

namespace DragonFruit.Six.Client.Overlays
{
    public partial class SearchProvider
    {
        private HxOffcanvas _searchOverlay;

        private SearchState CurrentState { get; set; }
        private UbisoftAccount DiscoveredAccount { get; set; }

        public async Task SearchForAccount(string username, Platform platform)
        {
            CurrentState = SearchState.Searching;
            await _searchOverlay.ShowAsync().ConfigureAwait(false);

            // do account search
            await Task.Delay(2500).ConfigureAwait(false);
            DiscoveredAccount = new UbisoftAccount
            {
                Username = "PaPa.Curry",
                Platform = Platform.PC,
                ProfileId = "14c01250-ef26-4a32-92ba-e04aa557d619",
                UbisoftId = "14c01250-ef26-4a32-92ba-e04aa557d619",
                PlatformId = "14c01250-ef26-4a32-92ba-e04aa557d619"
            };

            CurrentState = SearchState.Discovered;
            // await Task.Delay(1000).ConfigureAwait(false);

            // todo redirect to stats page
            // await _searchOverlay.HideAsync().ConfigureAwait(false);
        }
    }

    internal enum SearchState
    {
        /// <summary>
        /// Account search is in progress
        /// </summary>
        Searching,

        /// <summary>
        /// Account Discovered
        /// </summary>
        Discovered,

        /// <summary>
        /// User was not found
        /// </summary>
        NotFound,

        /// <summary>
        /// Other error occured (network or something similar)
        /// </summary>
        OtherError
    }
}
