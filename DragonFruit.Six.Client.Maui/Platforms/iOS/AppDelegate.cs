using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace DragonFruit.Six.Client.Maui
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}