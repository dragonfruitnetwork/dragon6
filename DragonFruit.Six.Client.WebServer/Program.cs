using System;
using System.Threading.Tasks;
using DragonFruit.Six.Client.Interop;
using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DragonFruit.Six.Client.WebServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

            builder.Services.AddHxServices();
            Activator.CreateInstance<Dragon6DeveloperServices>().InitialiseServices(builder.Services);

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
