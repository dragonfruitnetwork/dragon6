// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Threading.Tasks;

namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class WebViewInstallationService
    {
        public partial bool IsWebViewInstalled() => true;

        public partial Task<string> InstallWebView(IServiceProvider services)
        {
            return Task.FromResult("Install not supported on Android");
        }
    }
}
