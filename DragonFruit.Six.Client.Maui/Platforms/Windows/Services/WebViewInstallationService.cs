// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;
using DragonFruit.Data;
using DragonFruit.Data.Basic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class WebViewInstallationService
    {
        private const string WebView2Url = "https://go.microsoft.com/fwlink/p/?LinkId=2124703";
        private const string WebView2Installer = "MicrosoftEdgeWebview2Setup.exe";

        public partial bool IsWebViewInstalled()
        {
            var locations = new[]
            {
                (Registry.LocalMachine, @"SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}"),
                (Registry.CurrentUser, @"Software\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}")
            };

            foreach (var (key, path) in locations)
            {
                using var registry = key.OpenSubKey(path, RegistryRights.ReadKey);

                if (!string.IsNullOrEmpty((string)registry?.GetValue("pv")))
                {
                    return true;
                }
            }

            return false;
        }

        public async partial Task<string> InstallWebView(IServiceProvider services)
        {
            var installerLocation = Path.Combine(Path.GetTempPath(), WebView2Installer);
            await services.GetRequiredService<ApiClient>().PerformAsync(new BasicApiFileRequest(WebView2Url, installerLocation));

            if (!File.Exists(installerLocation))
            {
                return "One or more files could not be found. Please try again.";
            }

            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = installerLocation,
                Arguments = "/silent /install",
                UseShellExecute = false,
                CreateNoWindow = true
            });

            Debug.Assert(process != null);
            await process.WaitForExitAsync();

            return process.ExitCode != 0 ? $"Microsoft WebView2 failed to install (exit code {process.ExitCode}). Please try again or install manually." : null;
        }
    }
}
