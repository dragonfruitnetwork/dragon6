// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Client.Maui.Services;
using DragonFruit.Six.Client.Maui.WebView;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DragonFruit.Six.Client.Maui
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            MainPage = WebViewInstallationService.IsWebViewInstalled() ? new MainPage() : new WebViewInstallerPage();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

#if WINDOWS || MACCATALYST
            window.Width = 1300;
            window.Height = 900;
            window.Title = "Dragon6";
#endif

            return window;
        }
    }
}
