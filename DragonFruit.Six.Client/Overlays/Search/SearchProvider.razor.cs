// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Api.Utils;
using DragonFruit.Six.Client.Database.Services;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

#pragma warning disable CS4014

namespace DragonFruit.Six.Client.Overlays.Search
{
    public partial class SearchProvider : IDisposable
    {
        private HxOffcanvas _searchOverlay;

        private SearchState CurrentState { get; set; }

        [Inject]
        private IJSRuntime JavaRuntime { get; set; }

        [Inject]
        private AccountLookupCache Accounts { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private ILogger<SearchProvider> Logger { get; set; }

        [Inject]
        private SearchProviderState SearchProviderState { get; set; }

        protected override void OnInitialized()
        {
            SearchProviderState.AccountSearchRequested += TriggerAccountSearch;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            SearchForAccountAsync(SearchProviderState.LastUnhandledSearch);
        }

        private void TriggerAccountSearch(AccountSearchArgs args) => SearchForAccountAsync(args);

        /// <summary>
        /// Begins searching for an account.
        /// Causes the current window to be blocked until completed.
        /// </summary>
        private async Task SearchForAccountAsync(AccountSearchArgs args)
        {
            if (args == null)
            {
                return;
            }

            CurrentState = SearchState.Searching;
            await _searchOverlay.ShowAsync().ConfigureAwait(false);
            await Task.Delay(500).ConfigureAwait(false);

            try
            {
                var matchingAccounts = await Accounts.LookupAsync(new[] { args.Identifier }, args.Platform, args.IdentifierType).ConfigureAwait(false);

                // todo handle edge cases where multiple accounts are returned
                SearchProviderState.DiscoveredAccount = matchingAccounts.FirstOrDefault();
                CurrentState = SearchProviderState.DiscoveredAccount == null ? SearchState.NotFound : SearchState.Discovered;
            }
            catch (Exception e)
            {
                Logger.LogWarning("Error occured while searching for {user}: {@ex}", args, e);
                CurrentState = SearchState.OtherError;
            }

            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
            await Task.Delay(2500).ConfigureAwait(false);

            await _searchOverlay.HideAsync().ConfigureAwait(false);

            if (CurrentState == SearchState.Discovered)
            {
                Navigation.NavigateTo("/stats");
            }
        }

        public void Dispose()
        {
            SearchProviderState.AccountSearchRequested -= TriggerAccountSearch;
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
