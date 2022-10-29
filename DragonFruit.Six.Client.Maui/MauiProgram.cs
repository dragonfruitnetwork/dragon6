// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Api;
using DragonFruit.Six.Client.Database;
using DragonFruit.Six.Client.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace DragonFruit.Six.Client.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

            // maui webview services
            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            // blazor bootstrap services
            builder.Services.AddTransient<IFileSystemStructure, MauiFileSystemStructure>();
            builder.Services.AddSingleton<Dragon6Client, Dragon6DebugClient>();

            return builder.Build();
        }
    }
}
