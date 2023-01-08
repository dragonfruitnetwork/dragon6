// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Data;

namespace DragonFruit.Six.Client.Maui.WebView
{
    public partial class WebViewInstallerPage
    {
        public WebViewInstallerModel ViewModel { get; }

        public WebViewInstallerPage(ApiClient client)
        {
            ViewModel = new WebViewInstallerModel(this, client);

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.PrepareForUse.Execute(null);
        }
    }
}
