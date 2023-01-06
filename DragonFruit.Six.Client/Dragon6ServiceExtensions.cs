// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DragonFruit.Six.Api;
using DragonFruit.Six.Client.Configuration;
using DragonFruit.Six.Client.Database;
using DragonFruit.Six.Client.Database.Services;
using DragonFruit.Six.Client.Network;
using Havit.Blazor.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace DragonFruit.Six.Client
{
    public static class Dragon6ServiceExtensions
    {
        private const string ExternalServicesLocation = "DragonFruit.Six.Client.Services.dll";

        /// <summary>
        /// Registers core Dragon6 services with the <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services">The DI container to configure</param>
        public static void AddDragon6Services(this IServiceCollection services)
        {
            services.AddLogging(log => log.AddSerilog(CreateLogger(), true));

            // core services
            services.AddHxServices();
            services.AddSingleton<Dragon6Configuration>();
            services.AddAutoMapper(Dragon6EntityMapper.ConfigureMapper);

            var servicesLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ExternalServicesLocation);

            if (File.Exists(servicesLocation))
            {
                var targetType = Assembly.LoadFrom(servicesLocation).ExportedTypes.Single(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableTo(typeof(IDragon6ServiceInjector)));
                var injector = (IDragon6ServiceInjector)Activator.CreateInstance(targetType);

                injector.InitialiseServices(services);
            }
            else
            {
                services.AddSingleton<Dragon6Client, Dragon6DebugClient>();
            }

            // account caches and database services
            services.AddScoped<UserLookupCache>();
            services.AddScoped<AccountLookupCache>();
            services.AddSingleton<SavedPlayerRankService>();
        }

        private static Logger CreateLogger()
        {
            var config = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                         .MinimumLevel.Override("Microsoft.Extensions.Localization", LogEventLevel.Warning);

            config.WriteTo.Sentry(s =>
            {
                s.Dsn = "https://29ba62eac6314a3f8591a03785ca3403@o97031.ingest.sentry.io/6542292";
                s.Release = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3);

                s.BeforeSend = e =>
                {
                    if (Assembly.GetExecutingAssembly().GetName().Version > new Version(1, 0, 0))
                    {
                        return e;
                    }

                    return null;
                };

                s.MaxBreadcrumbs = 50;
                s.MinimumEventLevel = LogEventLevel.Error;
                s.MinimumBreadcrumbLevel = LogEventLevel.Debug;
            });

            config.WriteTo.Console(theme: AnsiConsoleTheme.Literate);

            return config.CreateLogger();
        }
    }
}
