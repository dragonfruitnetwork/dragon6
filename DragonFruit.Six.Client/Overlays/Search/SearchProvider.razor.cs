// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Threading.Tasks;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Accounts.Enums;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;

namespace DragonFruit.Six.Client.Overlays.Search
{
    public partial class SearchProvider
    {
        private HxOffcanvas _searchOverlay;

        private SearchState CurrentState { get; set; }

        [CascadingParameter]
        private SearchProviderState SearchProviderState { get; set; }

        /// <summary>
        /// Begins searching for an account. Causes the current window to be blocked until completed.
        /// </summary>
        /// <param name="identifier">The username or ubisoft id of the user to load</param>
        /// <param name="platform">The <see cref="Platform"/> the user is playing on</param>
        /// <param name="identifierType">The type of <see cref="identifier"/>, if known</param>
        public async Task SearchForAccount(string identifier, Platform platform, IdentifierType? identifierType = null)
        {
            CurrentState = SearchState.Searching;
            await _searchOverlay.ShowAsync().ConfigureAwait(false);

            // do account search
            identifierType ??= Guid.TryParse(identifier, out _) ? IdentifierType.UserId : IdentifierType.Name;

            await Task.Delay(2500).ConfigureAwait(false);
            SearchProviderState.DiscoveredAccount = new UbisoftAccount
            {
                Username = "PaPa.Curry",
                Platform = Platform.PC,
                ProfileId = "14c01250-ef26-4a32-92ba-e04aa557d619",
                UbisoftId = "14c01250-ef26-4a32-92ba-e04aa557d619",
                PlatformId = "14c01250-ef26-4a32-92ba-e04aa557d619"
            };

            CurrentState = SearchState.Discovered;
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);

            await Task.Delay(2000).ConfigureAwait(false);

            // todo redirect to stats page
            await _searchOverlay.HideAsync().ConfigureAwait(false);
        }

        private enum SearchState
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
}