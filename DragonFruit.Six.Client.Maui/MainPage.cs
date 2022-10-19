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
                        ComponentType = typeof(Dragon6Client)
                    }
                }
            };
        }
    }
}