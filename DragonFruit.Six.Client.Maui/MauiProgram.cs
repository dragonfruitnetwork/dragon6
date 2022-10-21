// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Client.Database;
using DragonFruit.Six.Client.Interop;
using Havit.Blazor.Components.Web;
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
            builder.Services.AddHxServices();
            builder.Services.AddTransient<IFileSystemStructure, MauiFileSystemStructure>();

            // todo locate DragonFruit.Six.Services.dll and load in IDragon6Services from that
            Activator.CreateInstance<Dragon6DeveloperServices>().InitialiseServices(builder.Services);

            return builder.Build();
        }
    }
}
