// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using Microsoft.Extensions.DependencyInjection;

namespace DragonFruit.Six.Client.Maui.WebView
{
    public partial class WebViewInstallerPage
    {
        public WebViewInstallerModel ViewModel { get; }

        public WebViewInstallerPage()
        {
            InitializeComponent();

            var ssf = Handler!.MauiContext!.Services.GetRequiredService<IServiceScopeFactory>();
            ViewModel = new WebViewInstallerModel(this, ssf);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.PrepareForUse.Execute(null);
        }
    }
}
