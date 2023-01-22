using System.Threading.Tasks;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Client.Database;
using DragonFruit.Six.Client.Overlays.Search;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DragonFruit.Six.Client.WebServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

            builder.Services.AddTransient<IDragon6Platform, WebServerPlatform>();
            builder.Services.AddDragon6Services();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToController("Index", "Host");

            await app.RunAsync().ConfigureAwait(false);
        }
    }
}
