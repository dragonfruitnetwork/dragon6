// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Client.Maui.Services;
using DragonFruit.Six.Client.Maui.WebView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DragonFruit.Six.Client.Maui
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            if (new WebViewInstallationService().IsWebViewInstalled())
            {
                MainPage = new MainPage();
            }
            else
            {
                var ssf = Handler.MauiContext!.Services.GetRequiredService<IServiceScopeFactory>();
                MainPage = new WebViewInstallerPage(ssf);
            }
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Width = 1200;
            window.Height = 900;
            window.Title = "Dragon6";

            return window;
        }
    }
}
