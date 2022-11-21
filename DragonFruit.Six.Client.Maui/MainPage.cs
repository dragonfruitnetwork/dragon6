// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Controls;

namespace DragonFruit.Six.Client.Maui
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Title = "Dragon6";
            Content = new BlazorWebView
            {
                HostPage = "wwwroot/index.html",
                RootComponents =
                {
                    new RootComponent
                    {
                        Selector = "#app",
                        ComponentType = typeof(Dragon6App)
                    },
                    new RootComponent
                    {
                        Selector = "head::after",
                        ComponentType = typeof(HeadOutlet)
                    }
                }
            };
        }
    }
}
