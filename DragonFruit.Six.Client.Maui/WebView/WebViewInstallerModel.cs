// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DragonFruit.Six.Client.Maui.Services;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using ReactiveUI;

namespace DragonFruit.Six.Client.Maui.WebView
{
    public class WebViewInstallerModel : ReactiveObject
    {
        private readonly Page _owner;
        private readonly IServiceScopeFactory _ssf;

        private readonly ObservableAsPropertyHelper<bool> _retryButtonVisible, _active;

        private string _errorMessage;

        public WebViewInstallerModel(Page owner, IServiceScopeFactory ssf)
        {
            _owner = owner;
            _ssf = ssf;

            var errorObserver = this.WhenValueChanged(x => x.ErrorMessage)
                                    .ObserveOn(RxApp.MainThreadScheduler)
                                    .Select(string.IsNullOrEmpty);

            _active = errorObserver.ToProperty(this, x => x.Active);
            _retryButtonVisible = errorObserver.Select(x => !x).ToProperty(this, x => x.RetryButtonVisible);

            PrepareForUse = ReactiveCommand.CreateFromTask(AttemptInstall);
        }

        public ICommand PrepareForUse { get; }

        public bool Active => _active.Value;
        public bool RetryButtonVisible => _retryButtonVisible.Value;

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        private async Task AttemptInstall()
        {
            ErrorMessage = null;

            using var scope = _ssf.CreateScope();
            ErrorMessage = await WebViewInstallationService.InstallWebView(scope.ServiceProvider);

            if (string.IsNullOrEmpty(ErrorMessage))
            {
                await _owner.Navigation.PushAsync(new MainPage(), true);
                _owner.Navigation.RemovePage(_owner);
            }
        }
    }
}
