using System.Threading.Tasks;
using DragonFruit.Six.Client.Database;
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

            builder.Services.AddTransient<IFileSystemStructure, WebServerFileSystemStructure>();
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
