// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Diagnostics.CodeAnalysis;
using DragonFruit.Six.Client.Configuration;
using DragonFruit.Six.Client.Database;
using DragonFruit.Six.Client.Maui.Services;
using DragonFruit.Six.Client.Overlays.Search;
using DragonFruit.Six.Client.Presence;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace DragonFruit.Six.Client.Maui
{
    public static class MauiProgram
    {
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HeadOutlet))]
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

            // maui webview services
            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            var platformInfo = new MauiPlatform();

            builder.Services.AddSingleton<IDragon6Platform>(platformInfo);
            builder.Services.AddScoped<ILegacyVersionMigrator, LegacyMigrationService>();

            var searchState = new SearchProviderState();
            builder.Services.AddSingleton(searchState);

            builder.Services.AddDragon6Services();

            // desktop-specific services
            if (platformInfo.CurrentPlatform.HasFlag(HostPlatform.Desktop))
            {
                builder.Services.AddSingleton<IPresenceClient, DiscordPresenceClient>();
            }

            builder.ConfigureLifecycleEvents(events =>
            {
#if ANDROID
                events.AddAndroid(android =>
                {
                    // because the intent can be triggered when the app is closed, handle both intents at the startup and when running
                    android.OnCreate((activity, _) => HandleIntent(activity.Intent));
                    android.OnNewIntent((_, intent) => HandleIntent(intent));

                    void HandleIntent(Android.Content.Intent intent)
                    {
                        if (intent?.Action != Android.Content.Intent.ActionView)
                            return;

                        if (!AccountSearchArgs.TryParseFromUrl(intent.DataString, out var searchArgs))
                            return;

                        searchState.TriggerSearch(searchArgs);
                    }
                });
#endif
            });

            return builder.Build();
        }
    }
}
