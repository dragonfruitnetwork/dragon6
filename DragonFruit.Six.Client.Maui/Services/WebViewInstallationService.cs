// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Threading.Tasks;

namespace DragonFruit.Six.Client.Maui.Services
{
    public static partial class WebViewInstallationService
    {
        public static partial bool IsWebViewInstalled();

        public static partial Task<string> InstallWebView(IServiceProvider services);
    }
}
