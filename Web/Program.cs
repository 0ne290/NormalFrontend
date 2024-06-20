using System.Text;
using Web.Daos;

namespace Web;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Report}/{action=Index}/{id?}");
        
        TelephoneSubscriberDao.WriteRandomTelephoneSubscribersToFile(Encoding.UTF8, "RandomTelephoneSubscribers.txt", 50);

        await app.RunAsync();
    }
}